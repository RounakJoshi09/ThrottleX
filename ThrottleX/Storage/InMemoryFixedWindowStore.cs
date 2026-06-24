using System.Collections.Concurrent;
using ThrottleX.Abstractions;

namespace ThrottleX.Storage;

/// <summary>
/// In-memory fixed-window counter state keyed by partition.
/// Thread-safe via per-entry locks and <see cref="ConcurrentDictionary{TKey,TValue}"/>.
/// </summary>
internal sealed class InMemoryFixedWindowStore : IRateLimitStore
{
    private readonly ConcurrentDictionary<string, FixedWindowEntry> _entries = new(StringComparer.Ordinal);

    /// <summary>
    /// Attempts to increment the counter for <paramref name="partitionKey"/> within the current window.
    /// </summary>
    public FixedWindowAcquireResult TryAcquire(
        string partitionKey,
        int permitCount,
        int permitLimit,
        TimeSpan window,
        DateTimeOffset utcNow)
    {
        ArgumentException.ThrowIfNullOrEmpty(partitionKey);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(permitCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(permitLimit);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(window, TimeSpan.Zero);

        if (permitCount > permitLimit)
        {
            var windowStart = AlignWindowStart(utcNow, window);
            var windowEnd = windowStart + window;
            return new FixedWindowAcquireResult(
                Acquired: false,
                Count: 0,
                Limit: permitLimit,
                WindowStartUtc: windowStart,
                WindowEndUtc: windowEnd,
                RejectionReason: Results.RateLimitRejectionReason.PermitCountExceedsLimit);
        }

        var entry = _entries.GetOrAdd(partitionKey, static _ => new FixedWindowEntry());
        lock (entry.Sync)
        {
            var windowStart = AlignWindowStart(utcNow, window);
            if (entry.WindowStartUtc != windowStart)
            {
                entry.WindowStartUtc = windowStart;
                entry.Count = 0;
            }

            entry.LastAccessedUtc = utcNow;
            var windowEnd = windowStart + window;

            if (entry.Count + permitCount > permitLimit)
            {
                return new FixedWindowAcquireResult(
                    Acquired: false,
                    Count: entry.Count,
                    Limit: permitLimit,
                    WindowStartUtc: windowStart,
                    WindowEndUtc: windowEnd,
                    RejectionReason: Results.RateLimitRejectionReason.LimitExceeded);
            }

            entry.Count += permitCount;
            return new FixedWindowAcquireResult(
                Acquired: true,
                Count: entry.Count,
                Limit: permitLimit,
                WindowStartUtc: windowStart,
                WindowEndUtc: windowEnd,
                RejectionReason: Results.RateLimitRejectionReason.None);
        }
    }

    /// <inheritdoc />
    public int PruneIdleEntries(TimeSpan idleThreshold, DateTimeOffset utcNow)
    {
        if (idleThreshold <= TimeSpan.Zero)
            return 0;

        var removed = 0;
        foreach (var (key, entry) in _entries)
        {
            DateTimeOffset lastAccess;
            lock (entry.Sync)
            {
                lastAccess = entry.LastAccessedUtc;
            }

            if (utcNow - lastAccess >= idleThreshold && _entries.TryRemove(key, out _))
                removed++;
        }

        return removed;
    }

    internal static DateTimeOffset AlignWindowStart(DateTimeOffset utcNow, TimeSpan window)
    {
        var ticks = utcNow.UtcTicks;
        var windowTicks = window.Ticks;
        var startTicks = ticks - (ticks % windowTicks);
        return new DateTimeOffset(startTicks, TimeSpan.Zero);
    }

    private sealed class FixedWindowEntry
    {
        public object Sync { get; } = new();
        public DateTimeOffset WindowStartUtc { get; set; }
        public int Count { get; set; }
        public DateTimeOffset LastAccessedUtc { get; set; }
    }
}

/// <summary>
/// Result of a fixed-window increment attempt.
/// </summary>
internal readonly record struct FixedWindowAcquireResult(
    bool Acquired,
    int Count,
    int Limit,
    DateTimeOffset WindowStartUtc,
    DateTimeOffset WindowEndUtc,
    Results.RateLimitRejectionReason RejectionReason);
