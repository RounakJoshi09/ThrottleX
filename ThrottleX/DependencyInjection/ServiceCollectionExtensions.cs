using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ThrottleX.Abstractions;
using ThrottleX.Internal;
using ThrottleX.Options;

namespace ThrottleX.DependencyInjection;

/// <summary>
/// Fluent builder returned from <see cref="ServiceCollectionExtensions.AddThrottleX(IServiceCollection)"/>.
/// </summary>
public sealed class ThrottleXBuilder
{
    internal ThrottleXBuilder(IServiceCollection services)
    {
        Services = services;
    }

    /// <summary>
    /// Underlying service collection for advanced registrations.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Registers or replaces a named rate-limit policy.
    /// </summary>
    /// <param name="name">Policy name (case-sensitive).</param>
    /// <param name="configure">Callback that configures the policy.</param>
    /// <returns>This builder for chaining.</returns>
    public ThrottleXBuilder AddPolicy(string name, Action<RateLimitPolicy> configure)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(configure);

        Services.PostConfigure<ThrottleXOptions>(options =>
        {
            if (!options.Policies.TryGetValue(name, out var policy))
            {
                policy = new RateLimitPolicy { Name = name };
                options.Policies[name] = policy;
            }
            else if (policy.Name is null)
            {
                policy.Name = name;
            }

            configure(policy);
        });

        return this;
    }

    /// <summary>
    /// Configures the default policy applied when no named policy is specified.
    /// </summary>
    /// <param name="configure">Callback that configures the default policy.</param>
    /// <returns>This builder for chaining.</returns>
    public ThrottleXBuilder ConfigureDefaultPolicy(Action<RateLimitPolicy> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        Services.PostConfigure<ThrottleXOptions>(options => configure(options.DefaultPolicy));
        return this;
    }
}

/// <summary>
/// Dependency injection helpers for ThrottleX core (no ASP.NET Core dependency).
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers ThrottleX with default options (100 requests / minute, fixed window).
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>A builder for registering named policies.</returns>
    public static ThrottleXBuilder AddThrottleX(this IServiceCollection services)
        => RegisterThrottleX(services, configure: null, configuration: null);

    /// <summary>
    /// Registers ThrottleX and configures options in code.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configure">Options configuration delegate.</param>
    /// <returns>A builder for registering named policies.</returns>
    public static ThrottleXBuilder AddThrottleX(this IServiceCollection services, Action<ThrottleXOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        return RegisterThrottleX(services, configure, configuration: null);
    }

    /// <summary>
    /// Registers ThrottleX and binds options from the <c>ThrottleX</c> configuration section.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration root or section.</param>
    /// <returns>A builder for registering named policies.</returns>
    /// <remarks>
    /// When <paramref name="configuration"/> is the application root, the <see cref="ThrottleXOptions.SectionName"/>
    /// child section is used. When it is already the ThrottleX section, it is bound directly.
    /// </remarks>
    public static ThrottleXBuilder AddThrottleX(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        return RegisterThrottleX(services, configure: null, configuration);
    }

    /// <summary>
    /// Registers ThrottleX, binds from configuration, then applies additional in-code configuration.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration root or section.</param>
    /// <param name="configure">Optional post-bind configuration.</param>
    /// <returns>A builder for registering named policies.</returns>
    public static ThrottleXBuilder AddThrottleX(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<ThrottleXOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(configure);
        return RegisterThrottleX(services, configure, configuration);
    }

    private static ThrottleXBuilder RegisterThrottleX(
        IServiceCollection services,
        Action<ThrottleXOptions>? configure,
        IConfiguration? configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions<ThrottleXOptions>();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<ThrottleXOptions>, ThrottleXOptionsValidator>());

        if (configuration is not null)
        {
            var section = ResolveThrottleXSection(configuration);
            services.AddOptions<ThrottleXOptions>().Bind(section);
        }

        if (configure is not null)
            services.Configure(configure);

        services.TryAddSingleton(TimeProvider.System);
        services.TryAddSingleton<IRateLimiterFactory, RateLimiterFactory>();
        services.TryAddSingleton<IRateLimitPolicyProvider, OptionsRateLimitPolicyProvider>();
        services.TryAddSingleton<PolicyRateLimiterResolver>();
        services.TryAddSingleton<IRateLimiter, DefaultPolicyRateLimiter>();

        return new ThrottleXBuilder(services);
    }

    private static IConfiguration ResolveThrottleXSection(IConfiguration configuration)
    {
        // If caller passed the app root, use the ThrottleX child section; otherwise bind the given section as-is.
        if (configuration is IConfigurationRoot)
            return configuration.GetSection(ThrottleXOptions.SectionName);

        // Heuristic: section already named ThrottleX (or bound object path ends with it).
        if (configuration is IConfigurationSection section &&
            string.Equals(section.Key, ThrottleXOptions.SectionName, StringComparison.OrdinalIgnoreCase))
        {
            return section;
        }

        var child = configuration.GetSection(ThrottleXOptions.SectionName);
        return child.Exists() ? child : configuration;
    }
}

/// <summary>
/// Service provider helpers for resolving named rate limiters.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Gets the rate limiter for the configured default policy.
    /// </summary>
    public static IRateLimiter GetThrottleXRateLimiter(this IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        return services.GetRequiredService<IRateLimiter>();
    }

    /// <summary>
    /// Gets (or creates) a rate limiter for the named policy.
    /// </summary>
    /// <param name="services">Service provider.</param>
    /// <param name="policyName">Registered policy name.</param>
    /// <exception cref="InvalidOperationException">Thrown when the policy name is unknown.</exception>
    public static IRateLimiter GetThrottleXRateLimiter(this IServiceProvider services, string policyName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrEmpty(policyName);
        return services.GetRequiredService<PolicyRateLimiterResolver>().GetRequired(policyName);
    }
}
