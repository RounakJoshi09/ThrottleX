namespace ThrottleX.Options;

/// <summary>
/// Supported rate-limiting algorithms.
/// </summary>
public enum RateLimitAlgorithm
{
    /// <summary>
    /// Fixed time windows with a hard permit count reset at each boundary.
    /// Simple and fast, but may allow a burst of up to 2x limit across a window boundary.
    /// </summary>
    FixedWindow = 0,

    /// <summary>
    /// Token bucket allowing controlled bursts up to capacity while enforcing a steady refill rate.
    /// </summary>
    TokenBucket = 1,

    /// <summary>
    /// Sliding window counter for smoother limiting without fixed-window boundary bursts.
    /// </summary>
    SlidingWindow = 2
}
