# [Phase 5] Per-endpoint throttle statistics

**Labels:** `phase-5`, `enhancement`, `observability`

## Phase
**Phase 5: Hot Configuration & Metrics** (Weeks 10–13)

## Summary
Extend metrics to break down throttle stats per endpoint (route/template), enabling operators to spot hot limited endpoints.

## Tasks
- [ ] Key metrics by endpoint identity (route pattern preferred over raw path)
- [ ] Include policy name and allow/reject counts per endpoint
- [ ] Expose in metrics JSON (e.g. `endpoints` array/object)
- [ ] Memory bounds: max tracked endpoints or LRU/eviction strategy
- [ ] Tests for multi-endpoint scenarios

## Acceptance criteria
- [ ] Two different endpoints show separate counters
- [ ] Metrics payload remains reasonable size under many routes (eviction/cap works)
- [ ] Documented in observability section of README/docs

## Dependencies
Requires: Metrics API (5.2)

## Parent
Tracks: Epic execution plan
