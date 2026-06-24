using Microsoft.Extensions.Options;
using ThrottleX.Abstractions;
using ThrottleX.Algorithms;
using ThrottleX.Options;

namespace ThrottleX.Internal;

/// <summary>
/// Default <see cref="IRateLimiterFactory"/> that builds in-memory limiters for supported algorithms.
/// </summary>
internal sealed class RateLimiterFactory : IRateLimiterFactory
{
    private readonly TimeProvider _timeProvider;

    public RateLimiterFactory(TimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    public IRateLimiter Create(RateLimitPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);

        return policy.Algorithm switch
        {
            RateLimitAlgorithm.FixedWindow => new FixedWindowRateLimiter(policy, _timeProvider),
            RateLimitAlgorithm.TokenBucket => throw new NotSupportedException(
                "TokenBucket is not implemented yet. Use FixedWindow or wait for Phase 2."),
            RateLimitAlgorithm.SlidingWindow => throw new NotSupportedException(
                "SlidingWindow is not implemented yet. Use FixedWindow or wait for Phase 2."),
            _ => throw new NotSupportedException($"Algorithm '{policy.Algorithm}' is not supported.")
        };
    }
}

/// <summary>
/// Options-backed <see cref="IRateLimitPolicyProvider"/>.
/// </summary>
internal sealed class OptionsRateLimitPolicyProvider : IRateLimitPolicyProvider
{
    private readonly ThrottleXOptions _options;
    private readonly IReadOnlyDictionary<string, RateLimitPolicy> _policies;

    public OptionsRateLimitPolicyProvider(IOptions<ThrottleXOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        _options = optionsAccessor.Value ?? throw new ArgumentException("ThrottleXOptions value is null.", nameof(optionsAccessor));
        _policies = new Dictionary<string, RateLimitPolicy>(_options.Policies, StringComparer.Ordinal);
        DefaultPolicy = _options.DefaultPolicy ?? new RateLimitPolicy { Name = "default" };
    }

    public RateLimitPolicy DefaultPolicy { get; }

    public RateLimitPolicy? GetPolicy(string policyName)
    {
        if (string.IsNullOrEmpty(policyName))
            return null;

        if (_policies.TryGetValue(policyName, out var policy))
            return policy;

        if (string.Equals(DefaultPolicy.Name, policyName, StringComparison.Ordinal))
            return DefaultPolicy;

        return null;
    }

    public IReadOnlyDictionary<string, RateLimitPolicy> GetAllPolicies() => _policies;
}

/// <summary>
/// Default <see cref="IRateLimiter"/> that applies the configured default policy.
/// </summary>
internal sealed class DefaultPolicyRateLimiter : IRateLimiter
{
    private readonly IRateLimiter _inner;

    public DefaultPolicyRateLimiter(IRateLimiterFactory factory, IRateLimitPolicyProvider policyProvider)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(policyProvider);
        _inner = factory.Create(policyProvider.DefaultPolicy);
    }

    public ValueTask<Results.RateLimitLease> AcquireAsync(
        string partitionKey,
        int permitCount = 1,
        CancellationToken cancellationToken = default) =>
        _inner.AcquireAsync(partitionKey, permitCount, cancellationToken);
}

/// <summary>
/// Resolves a limiter for a named policy, creating and caching instances per policy name.
/// </summary>
internal sealed class PolicyRateLimiterResolver
{
    private readonly IRateLimiterFactory _factory;
    private readonly IRateLimitPolicyProvider _policyProvider;
    private readonly System.Collections.Concurrent.ConcurrentDictionary<string, IRateLimiter> _cache = new(StringComparer.Ordinal);

    public PolicyRateLimiterResolver(IRateLimiterFactory factory, IRateLimitPolicyProvider policyProvider)
    {
        _factory = factory;
        _policyProvider = policyProvider;
    }

    public IRateLimiter GetDefault() =>
        _cache.GetOrAdd("__default__", _ => _factory.Create(_policyProvider.DefaultPolicy));

    public IRateLimiter GetRequired(string policyName)
    {
        ArgumentException.ThrowIfNullOrEmpty(policyName);
        return _cache.GetOrAdd(policyName, name =>
        {
            var policy = _policyProvider.GetPolicy(name)
                ?? throw new InvalidOperationException($"Rate limit policy '{name}' was not found.");
            return _factory.Create(policy);
        });
    }
}
