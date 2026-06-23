# [Phase 1] Core abstractions and interfaces

**Labels:** `phase-1`, `foundation`, `enhancement`

## Phase
**Phase 1: Foundation** (Weeks 1–2)

## Summary
Define the public contracts and option types that all algorithms, stores, and middleware will implement. This is the architectural backbone of ThrottleX.

## Proposed types (adjust as design solidifies)

### Core contracts
```csharp
// Acquire a permit for a given partition key (e.g. IP, user id, endpoint)
public interface IRateLimiter
{
    ValueTask<RateLimitLease> AcquireAsync(
        string partitionKey,
        int permitCount = 1,
        CancellationToken cancellationToken = default);
}

// Pluggable storage backend
public interface IRateLimitStore
{
    // Counter / token / window operations as needed by algorithms
}

// Policy resolution (named policies, endpoint metadata)
public interface IRateLimitPolicyProvider
{
    RateLimitPolicy? GetPolicy(string policyName);
}
```

### Result / metadata types
- `RateLimitLease` — `IsAcquired`, `RetryAfter`, remaining tokens/requests
- `RateLimitResult` / rejection reason enum
- `RateLimitHeaders` constants for standard response headers

### Options / policy model
- `ThrottleXOptions` — global defaults
- `RateLimitPolicy` — algorithm type, permit limit, window/refill settings
- `RateLimitAlgorithm` enum — `FixedWindow`, `TokenBucket`, `SlidingWindow`
- Partition key strategies (interface or delegates): IP, user, endpoint, composite

## Tasks
- [ ] Create folder layout under `ThrottleX/` (`Abstractions/`, `Options/`, `Results/`, `Internal/`)
- [ ] Implement interfaces and POCOs with XML documentation
- [ ] Define algorithm-specific option types if needed (or single unified policy config)
- [ ] Add internal helpers only if required by interfaces (keep lean)
- [ ] Remove placeholder `Class1.cs`

## Acceptance criteria
- [ ] Public API compiles with nullable enabled
- [ ] XML docs on all public types/members
- [ ] No algorithm implementation in this issue (interfaces only)
- [ ] Design supports both in-memory and Redis backends without leaking store details upward

## Dependencies
Blocks: Fixed Window, DI extensions, all later algorithm/middleware work

## Parent
Tracks: Epic execution plan
