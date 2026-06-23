# [Phase 1] Fixed Window Counter (in-memory)

**Labels:** `phase-1`, `foundation`, `enhancement`, `algorithm`

## Phase
**Phase 1: Foundation** (Weeks 1–2)

## Summary
Implement the Fixed Window Counter algorithm backed by an in-memory store. This is the first complete rate limiter and validates the abstraction design.

## Algorithm behavior
- Time is divided into fixed windows (e.g. 1 minute)
- Each window allows up to `N` requests for a given partition key
- Counter resets at window boundary (may allow burst at boundary — document this trade-off)
- Must be thread-safe for concurrent requests in the same process

## Tasks
- [ ] Implement `InMemoryRateLimitStore` (or algorithm-specific in-memory state)
- [ ] Implement `FixedWindowRateLimiter` implementing `IRateLimiter`
- [ ] Support configurable window size and permit limit via policy/options
- [ ] Return accurate `IsAcquired`, remaining count, and `RetryAfter` on rejection
- [ ] Handle partition key isolation (different keys = independent counters)
- [ ] Consider TTL/cleanup strategy for idle keys (prevent unbounded memory growth)

## Acceptance criteria
- [ ] Correctly allows requests within limit; rejects when limit exceeded
- [ ] Window resets at the correct boundary
- [ ] Thread-safe under parallel `AcquireAsync` calls
- [ ] XML docs describe burst-at-boundary behavior

## Dependencies
Requires: Core abstractions
Blocks: Fixed Window unit tests, DI registration of this limiter

## Parent
Tracks: Epic execution plan
