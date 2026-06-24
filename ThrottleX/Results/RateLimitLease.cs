namespace ThrottleX.Results;

/// <summary>
/// Outcome of a rate-limit acquisition attempt.
/// </summary>
public sealed class RateLimitLease
{
    /// <summary>
    /// <c>true</c> when permits were granted; <c>false</c> when the request should be rejected (e.g. HTTP 429).
    /// </summary>
    public bool IsAcquired { get; init; }

    /// <summary>
    /// Permits/tokens remaining after this evaluation (may be zero when rejected).
    /// </summary>
    public long Remaining { get; init; }

    /// <summary>
    /// Configured maximum permits/tokens for the active window or bucket.
    /// </summary>
    public long Limit { get; init; }

    /// <summary>
    /// Suggested wait before retrying when not acquired; <c>null</c> when acquired or unknown.
    /// </summary>
    public TimeSpan? RetryAfter { get; init; }

    /// <summary>
    /// UTC instant when the current fixed/sliding window ends, when applicable.
    /// </summary>
    public DateTimeOffset? WindowResetUtc { get; init; }

    /// <summary>
    /// Partition key that was evaluated.
    /// </summary>
    public string PartitionKey { get; init; } = string.Empty;

    /// <summary>
    /// Rejection reason when <see cref="IsAcquired"/> is <c>false</c>.
    /// </summary>
    public RateLimitRejectionReason RejectionReason { get; init; } = RateLimitRejectionReason.None;

    /// <summary>
    /// Creates a successful lease.
    /// </summary>
    public static RateLimitLease Acquired(
        string partitionKey,
        long remaining,
        long limit,
        DateTimeOffset? windowResetUtc = null) =>
        new()
        {
            IsAcquired = true,
            PartitionKey = partitionKey,
            Remaining = remaining,
            Limit = limit,
            WindowResetUtc = windowResetUtc,
            RejectionReason = RateLimitRejectionReason.None
        };

    /// <summary>
    /// Creates a rejected lease.
    /// </summary>
    public static RateLimitLease Rejected(
        string partitionKey,
        long remaining,
        long limit,
        TimeSpan? retryAfter,
        RateLimitRejectionReason reason,
        DateTimeOffset? windowResetUtc = null) =>
        new()
        {
            IsAcquired = false,
            PartitionKey = partitionKey,
            Remaining = remaining,
            Limit = limit,
            RetryAfter = retryAfter,
            WindowResetUtc = windowResetUtc,
            RejectionReason = reason
        };
}
