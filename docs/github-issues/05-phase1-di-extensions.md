# [Phase 1] DI extension methods (AddThrottleX)

**Labels:** `phase-1`, `foundation`, `enhancement`

## Phase
**Phase 1: Foundation** (Weeks 1–2)

## Summary
Provide `Microsoft.Extensions.DependencyInjection` integration so consumers can register ThrottleX with sensible defaults in any .NET host (console, worker, ASP.NET Core).

## Proposed API (preview)
```csharp
services.AddThrottleX(options =>
{
    options.DefaultPolicy = new RateLimitPolicy
    {
        Algorithm = RateLimitAlgorithm.FixedWindow,
        PermitLimit = 100,
        Window = TimeSpan.FromMinutes(1)
    };
});

// Or register named policies
services.AddThrottleX()
    .AddPolicy("api", policy => { ... })
    .AddPolicy("auth", policy => { ... });
```

## Tasks
- [ ] Add appropriate `Microsoft.Extensions.*` package references to core (or thin hosting package — prefer core if lightweight)
- [ ] Implement `ServiceCollectionExtensions.AddThrottleX(...)`
- [ ] Register `IRateLimiter` / policy provider / store with correct lifetimes
- [ ] Support configuration binding from `IConfiguration` section (`"ThrottleX"`)
- [ ] Document minimal setup in README quick-start section

## Acceptance criteria
- [ ] Can resolve a working `IRateLimiter` from `IServiceProvider` after `AddThrottleX`
- [ ] Options validate obvious misconfiguration (e.g. zero/negative limits) where reasonable
- [ ] No ASP.NET Core dependency in core DI (middleware lives in `ThrottleX.AspNetCore`)

## Dependencies
Requires: Core abstractions, at least one algorithm (Fixed Window)

## Parent
Tracks: Epic execution plan
