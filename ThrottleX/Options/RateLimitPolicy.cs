namespace ThrottleX.Options;

/// <summary>
/// Describes how rate limiting is applied for a policy (algorithm, limits, and timing).
/// </summary>
public sealed class RateLimitPolicy
{
    /// <summary>
    /// Optional display name for diagnostics and configuration binding.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Algorithm used to evaluate permits.
    /// </summary>
    public RateLimitAlgorithm Algorithm { get; set; } = RateLimitAlgorithm.FixedWindow;

    /// <summary>
    /// Maximum permits allowed per window (fixed/sliding window) or maximum tokens in the bucket (token bucket).
    /// Must be greater than zero.
    /// </summary>
    public int PermitLimit { get; set; } = 100;

    /// <summary>
    /// Window duration for fixed/sliding window algorithms. Must be positive for those algorithms.
    /// </summary>
    public TimeSpan Window { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Token bucket capacity (maximum tokens held at once). Defaults to <see cref="PermitLimit"/> when zero.
    /// </summary>
    public int TokenCapacity { get; set; }

    /// <summary>
    /// Tokens refilled per second for token bucket. When zero, derived from <see cref="PermitLimit"/> / <see cref="Window"/>.
    /// </summary>
    public double RefillTokensPerSecond { get; set; }

    /// <summary>
    /// Initial tokens when a token-bucket partition is first seen. Defaults to full capacity when negative.
    /// </summary>
    public int InitialTokens { get; set; } = -1;

    /// <summary>
    /// How long idle partition keys are retained in memory before cleanup. Defaults to 10 minutes.
    /// </summary>
    public TimeSpan IdleEntryTtl { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Creates a shallow copy of this policy.
    /// </summary>
    public RateLimitPolicy Clone() => new()
    {
        Name = Name,
        Algorithm = Algorithm,
        PermitLimit = PermitLimit,
        Window = Window,
        TokenCapacity = TokenCapacity,
        RefillTokensPerSecond = RefillTokensPerSecond,
        InitialTokens = InitialTokens,
        IdleEntryTtl = IdleEntryTtl
    };
}
