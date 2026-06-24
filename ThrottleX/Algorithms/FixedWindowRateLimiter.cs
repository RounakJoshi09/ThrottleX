using ThrottleX.Abstractions;
using ThrottleX.Options;
using ThrottleX.Results;
using ThrottleX.Storage;

namespace ThrottleX.Algorithms;

/// <summary>
/// In-process fixed window counter rate limiter.
/// </summary>
/// <remarks>
/// <para>
/// Time is divided into fixed windows of length <see cref="RateLimitPolicy.Window"/>.
/// Each partition key may consume up to <see cref="RateLimitPolicy.PermitLimit"/> permits per window;
/// the counter resets at the next window boundary.
/// </para>
/// <para>
/// <b>Burst at boundary:</b> because each window resets independently, a client may consume the full
/// limit at the end of one window and again at the start of the next, briefly allowing up to ~2x the
/// configured rate across the boundary. Prefer token bucket or sliding window when that matters.
/// </para>
/// </remarks>
public sealed class FixedWindowRateLimiter : IRateLimiter
{
    private readonly InMemoryFixedWindowStore _store;
    private readonly RateLimitPolicy _policy;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Creates a fixed-window rate limiter with a dedicated in-memory store.
    /// </summary>
    /// <param name="policy">Policy providing permit limit and window duration.</param>
    /// <param name="timeProvider">Clock used for window alignment (inject for tests).</param>
    public FixedWindowRateLimiter(RateLimitPolicy policy, TimeProvider? timeProvider = null)
        : this(policy, timeProvider, store: null)
    {
    }

    internal FixedWindowRateLimiter(
        RateLimitPolicy policy,
        TimeProvider? timeProvider,
        InMemoryFixedWindowStore? store)
    {
        ArgumentNullException.ThrowIfNull(policy);
        if (policy.PermitLimit <= 0)
            throw new ArgumentOutOfRangeException(nameof(policy), "PermitLimit must be greater than zero.");
        if (policy.Window <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(policy), "Window must be positive.");

        _policy = policy.Clone();
        _timeProvider = timeProvider ?? TimeProvider.System;
        _store = store ?? new InMemoryFixedWindowStore();
    }

    /// <inheritdoc />
    public ValueTask<RateLimitLease> AcquireAsync(
        string partitionKey,
        int permitCount = 1,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentException.ThrowIfNullOrEmpty(partitionKey);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(permitCount);

        var utcNow = _timeProvider.GetUtcNow();
        var result = _store.TryAcquire(
            partitionKey,
            permitCount,
            _policy.PermitLimit,
            _policy.Window,
            utcNow);

        var remaining = Math.Max(0, result.Limit - result.Count);
        var retryAfter = result.Acquired
            ? (TimeSpan?)null
            : result.WindowEndUtc > utcNow
                ? result.WindowEndUtc - utcNow
                : TimeSpan.Zero;

        if (result.Acquired)
        {
            return ValueTask.FromResult(RateLimitLease.Acquired(
                partitionKey,
                remaining,
                result.Limit,
                result.WindowEndUtc));
        }

        return ValueTask.FromResult(RateLimitLease.Rejected(
            partitionKey,
            remaining,
            result.Limit,
            retryAfter,
            result.RejectionReason,
            result.WindowEndUtc));
    }

    /// <summary>
    /// Removes idle partition entries from the in-memory store.
    /// </summary>
    public int PruneIdleEntries(TimeSpan? idleThreshold = null)
    {
        var threshold = idleThreshold ?? _policy.IdleEntryTtl;
        return _store.PruneIdleEntries(threshold, _timeProvider.GetUtcNow());
    }
}
