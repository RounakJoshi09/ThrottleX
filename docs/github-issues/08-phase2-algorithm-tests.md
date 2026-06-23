# [Phase 2] Comprehensive algorithm test coverage

**Labels:** `phase-2`, `testing`

## Phase
**Phase 2: Core Algorithms** (Weeks 3–4)

## Summary
Expand `ThrottleX.Tests` to fully cover Token Bucket and Sliding Window, plus cross-algorithm contract tests so all limiters behave consistently at the `IRateLimiter` boundary.

## Tasks
- [ ] Token Bucket tests: burst capacity, refill over time, reject when empty, partition isolation
- [ ] Sliding Window tests: smooth boundary behavior, limit enforcement, partition isolation
- [ ] Shared/contract tests (optional parameterized theory): all algorithms implement same lease fields
- [ ] Edge cases: `permitCount > 1`, zero/invalid options (if validation throws)
- [ ] Use `TimeProvider` / fake clock for deterministic timing tests
- [ ] Aim for meaningful coverage on algorithm + in-memory store code paths

## Acceptance criteria
- [ ] `dotnet test` green with all algorithm suites
- [ ] Tests are deterministic (no flaky timing sleeps unless unavoidable and documented)
- [ ] Failures produce clear assertion messages

## Dependencies
Requires: Token Bucket, Sliding Window implementations; Fixed Window tests as pattern

## Parent
Tracks: Epic execution plan
