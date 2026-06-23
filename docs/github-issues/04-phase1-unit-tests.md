# [Phase 1] Unit test project + Fixed Window tests

**Labels:** `phase-1`, `foundation`, `testing`

## Phase
**Phase 1: Foundation** (Weeks 1–2)

## Summary
Stand up `ThrottleX.Tests` with xUnit and establish patterns for algorithm testing. Cover Fixed Window thoroughly as the reference test suite for later algorithms.

## Tasks
- [ ] Add `ThrottleX.Tests` xUnit project targeting `net9.0`
- [ ] Reference core `ThrottleX` project
- [ ] Add test helpers/fixtures (fake clock/time provider if needed for determinism)
- [ ] Fixed Window tests:
  - [ ] Allows requests under limit
  - [ ] Rejects when limit exceeded
  - [ ] Resets after window expires
  - [ ] Isolates partition keys
  - [ ] Concurrent access does not over-admit beyond limit (or documents tolerance)
  - [ ] `RetryAfter` / remaining counts are sensible
- [ ] Ensure `dotnet test` is documented in README (already sketched)

## Acceptance criteria
- [ ] `dotnet test` passes in CI-ready fashion (local first)
- [ ] Tests are deterministic (inject `TimeProvider` if feasible on .NET 9)
- [ ] Clear naming: `FixedWindowRateLimiterTests` (or similar)

## Dependencies
Requires: Core abstractions, Fixed Window implementation

## Parent
Tracks: Epic execution plan
