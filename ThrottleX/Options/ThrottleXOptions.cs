namespace ThrottleX.Options;

/// <summary>
/// Global ThrottleX configuration bound from the <c>ThrottleX</c> configuration section or configured in code.
/// </summary>
public sealed class ThrottleXOptions
{
    /// <summary>
    /// Configuration section name used with <c>IConfiguration</c> binding.
    /// </summary>
    public const string SectionName = "ThrottleX";

    /// <summary>
    /// Default policy applied when no named policy is requested.
    /// </summary>
    public RateLimitPolicy DefaultPolicy { get; set; } = new()
    {
        Name = "default",
        Algorithm = RateLimitAlgorithm.FixedWindow,
        PermitLimit = 100,
        Window = TimeSpan.FromMinutes(1)
    };

    /// <summary>
    /// Named policies keyed by policy name (case-sensitive).
    /// </summary>
    public Dictionary<string, RateLimitPolicy> Policies { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// Interval between background idle-entry pruning passes. Set to <see cref="Timeout.InfiniteTimeSpan"/> to disable.
    /// </summary>
    public TimeSpan PruneInterval { get; set; } = TimeSpan.FromMinutes(5);
}
