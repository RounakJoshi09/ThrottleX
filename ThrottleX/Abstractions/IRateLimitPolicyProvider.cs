namespace ThrottleX.Abstractions;

/// <summary>
/// Resolves named <see cref="Options.RateLimitPolicy"/> instances configured at startup or via configuration.
/// </summary>
public interface IRateLimitPolicyProvider
{
    /// <summary>
    /// Gets the policy registered under <paramref name="policyName"/>, or <c>null</c> if not found.
    /// </summary>
    /// <param name="policyName">Policy name (case-sensitive).</param>
    /// <returns>The policy, or <c>null</c>.</returns>
    Options.RateLimitPolicy? GetPolicy(string policyName);

    /// <summary>
    /// Gets the default policy used when no named policy is specified.
    /// </summary>
    Options.RateLimitPolicy DefaultPolicy { get; }

    /// <summary>
    /// Enumerates all registered named policies (excluding the default unless also registered by name).
    /// </summary>
    IReadOnlyDictionary<string, Options.RateLimitPolicy> GetAllPolicies();
}
