# [Phase 2] Sliding Window Counter (in-memory)

**Labels:** `phase-2`, `enhancement`, `algorithm`

## Phase
**Phase 2: Core Algorithms** (Weeks 3–4)

## Summary
Implement Sliding Window Counter in-memory. Smooths the Fixed Window boundary burst problem by weighting current and previous window counts.

## Algorithm behavior (common approach)
- Maintain counts for current fixed window and previous window
- Effective count ≈ `prevCount * (1 - elapsed/window) + currentCount`
- Reject when effective count would exceed `permitLimit`
- Alternative: precise sliding log of timestamps (more accurate, more memory)—choose and document

## Tasks
- [ ] Implement `SlidingWindowRateLimiter` implementing `IRateLimiter`
- [ ] Choose and document algorithm variant (weighted counter vs timestamp log)
- [ ] Configurable window size and permit limit
- [ ] Accurate `RetryAfter` / remaining estimate
- [ ] Partition key isolation + cleanup strategy
- [ ] Thread-safety

## Acceptance criteria
- [ ] Reduces or eliminates double-burst at fixed window boundaries vs Fixed Window (with tests/docs)
- [ ] Under steady load, admits approximately `permitLimit` per window
- [ ] Consistent `IRateLimiter` result semantics with other algorithms

## Dependencies
Requires: Core abstractions
Related: Fixed Window (compare behavior in docs/tests)

## Parent
Tracks: Epic execution plan
