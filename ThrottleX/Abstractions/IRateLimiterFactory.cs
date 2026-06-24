namespace ThrottleX.Abstractions;

/// <summary>
/// Factory that creates <see cref="IRateLimiter"/> instances for a given <see cref="Options.RateLimitPolicy"/>.
/// </summary>
public interface IRateLimiterFactory
{
    /// <summary>
    /// Creates a rate limiter configured for the specified policy.
    /// </summary>
    /// <param name="policy">Policy describing algorithm and limits.</param>
    /// <returns>A configured <see cref="IRateLimiter"/>.</returns>
    IRateLimiter Create(Options.RateLimitPolicy policy);
}
