# [Phase 4] Multi-instance / race condition integration tests

**Labels:** `phase-4`, `redis`, `testing`

## Phase
**Phase 4: Distributed Support** (Weeks 7–9)

## Summary
Validate distributed correctness under concurrent access from multiple logical clients/instances, focusing on race conditions and over-admission.

## Tasks
- [ ] Integration test harness (shared Redis connection; optional Testcontainers)
- [ ] Concurrent acquires from multiple limiter instances (same keys/policies)
- [ ] Assert total success ≤ permit limit (per algorithm)
- [ ] Optional: chaos—brief Redis blip, assert configured fail-open/closed behavior
- [ ] Document how to run tests locally (Redis requirement)
- [ ] Mark tests `[Trait("Category", "Integration")]` or similar for optional CI filtering

## Acceptance criteria
- [ ] Integration suite passes with local/docker Redis
- [ ] Failures clearly indicate which algorithm/scenario broke
- [ ] README or `docs/` notes prerequisites

## Dependencies
Requires: Redis Fixed Window, Token Bucket, Sliding Window (4.2–4.4)

## Parent
Tracks: Epic execution plan
