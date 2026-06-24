using Microsoft.Extensions.Options;
using ThrottleX.Options;

namespace ThrottleX.Internal;

/// <summary>
/// Validates <see cref="ThrottleXOptions"/> at options bind/build time.
/// </summary>
internal sealed class ThrottleXOptionsValidator : IValidateOptions<ThrottleXOptions>
{
    public ValidateOptionsResult Validate(string? name, ThrottleXOptions options)
    {
        if (options is null)
            return ValidateOptionsResult.Fail("ThrottleXOptions must not be null.");

        var errors = new List<string>();
        ValidatePolicy(options.DefaultPolicy, "DefaultPolicy", errors);

        foreach (var (policyName, policy) in options.Policies)
        {
            if (string.IsNullOrWhiteSpace(policyName))
                errors.Add("Policy keys must be non-empty.");
            else
                ValidatePolicy(policy, $"Policies['{policyName}']", errors);
        }

        if (options.PruneInterval < TimeSpan.Zero && options.PruneInterval != Timeout.InfiniteTimeSpan)
            errors.Add("PruneInterval must be non-negative or InfiniteTimeSpan.");

        return errors.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(errors);
    }

    internal static void ValidatePolicy(RateLimitPolicy policy, string path, List<string> errors)
    {
        if (policy is null)
        {
            errors.Add($"{path} must not be null.");
            return;
        }

        if (policy.PermitLimit <= 0)
            errors.Add($"{path}.PermitLimit must be greater than zero.");

        if (policy.IdleEntryTtl <= TimeSpan.Zero)
            errors.Add($"{path}.IdleEntryTtl must be positive.");

        switch (policy.Algorithm)
        {
            case RateLimitAlgorithm.FixedWindow:
            case RateLimitAlgorithm.SlidingWindow:
                if (policy.Window <= TimeSpan.Zero)
                    errors.Add($"{path}.Window must be positive for {policy.Algorithm}.");
                break;

            case RateLimitAlgorithm.TokenBucket:
                var capacity = policy.TokenCapacity > 0 ? policy.TokenCapacity : policy.PermitLimit;
                if (capacity <= 0)
                    errors.Add($"{path}.TokenCapacity (or PermitLimit) must be greater than zero for TokenBucket.");

                var refill = policy.RefillTokensPerSecond;
                if (refill <= 0 && policy.Window <= TimeSpan.Zero)
                    errors.Add($"{path}.RefillTokensPerSecond must be positive, or Window must be set to derive a rate.");
                else if (refill <= 0 && policy.Window > TimeSpan.Zero && policy.PermitLimit <= 0)
                    errors.Add($"{path}.Cannot derive refill rate: PermitLimit must be positive when RefillTokensPerSecond is unset.");
                break;

            default:
                errors.Add($"{path}.Algorithm value '{policy.Algorithm}' is not supported.");
                break;
        }
    }
}
