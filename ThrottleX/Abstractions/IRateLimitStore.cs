namespace ThrottleX.Abstractions;

/// <summary>
/// Pluggable storage backend for rate-limit state. Algorithms may use concrete stores
/// directly; this interface supports future distributed backends (e.g. Redis) without
/// leaking store details into middleware consumers.
/// </summary>
public interface IRateLimitStore
{
    /// <summary>
    /// Removes idle entries older than <paramref name="idleThreshold"/> relative to <paramref name="utcNow"/>.
    /// </summary>
    /// <param name="idleThreshold">Minimum age before an entry is considered idle.</param>
    /// <param name="utcNow">Current UTC time used for idle calculations.</param>
    /// <returns>Number of entries removed.</returns>
    int PruneIdleEntries(TimeSpan idleThreshold, DateTimeOffset utcNow);
}
