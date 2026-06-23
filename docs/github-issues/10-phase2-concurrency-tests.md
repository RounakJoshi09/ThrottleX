# [Phase 2] Thread-safety & concurrency tests

**Labels:** `phase-2`, `testing`

## Phase
**Phase 2: Core Algorithms** (Weeks 3–4)

## Summary
Stress in-memory limiters under high parallelism to validate thread-safety and document admission guarantees (strict vs best-effort).

## Tasks
- [ ] Parallel acquire tests: many tasks, same partition key, fixed permit limit
- [ ] Assert total successful acquires ≤ limit (or within documented tolerance)
- [ ] Parallel tests across many partition keys (no cross-key interference)
- [ ] Optional: stress test memory/cleanup under many keys
- [ ] Document concurrency guarantees in algorithm/class XML docs

## Acceptance criteria
- [ ] No data corruption / exceptions under parallel load
- [ ] Over-admission either prevented or explicitly documented with rationale
- [ ] Tests complete in reasonable time (avoid multi-minute CI)

## Dependencies
Requires: Comprehensive algorithm tests (2.3); all three in-memory algorithms preferred

## Parent
Tracks: Epic execution plan
