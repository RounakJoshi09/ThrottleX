namespace ThrottleX.Abstractions;

/// <summary>
/// Acquires rate-limit permits for a partition key (e.g. IP address, user id, or endpoint).
/// </summary>
public interface IRateLimiter
{
    /// <summary>
    /// Attempts to acquire the specified number of permits for <paramref name="partitionKey"/>.
    /// </summary>
    /// <param name="partitionKey">Logical bucket key; different keys are isolated.</param>
    /// <param name="permitCount">Number of permits to consume. Must be positive.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A lease describing whether the request was allowed and limit metadata.</returns>
    ValueTask<Results.RateLimitLease> AcquireAsync(
        string partitionKey,
        int permitCount = 1,
        CancellationToken cancellationToken = default);
}
