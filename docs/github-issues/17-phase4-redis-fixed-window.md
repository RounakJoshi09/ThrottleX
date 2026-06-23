# [Phase 4] Redis Fixed Window (Lua atomic ops)

**Labels:** `phase-4`, `redis`, `enhancement`, `algorithm`

## Phase
**Phase 4: Distributed Support** (Weeks 7–9)

## Summary
Implement distributed Fixed Window using Redis with Lua scripts for atomic increment + window TTL, ensuring multi-instance correctness.

## Tasks
- [ ] Lua script: increment counter, set TTL on first hit, return count + remaining + ttl
- [ ] `RedisFixedWindowRateLimiter` (or store method used by shared limiter)
- [ ] Map results to `RateLimitLease` (`IsAcquired`, `RetryAfter`, remaining)
- [ ] Handle Redis errors (fail-open vs fail-closed—document default, make configurable if needed)
- [ ] Unit/integration tests (Redis required or Testcontainers/emulator—document)

## Acceptance criteria
- [ ] Two app instances sharing Redis cannot exceed limit (within one window)
- [ ] Behavior aligns with in-memory Fixed Window semantics as closely as possible
- [ ] Keys expire (no unbounded growth for idle partitions)

## Dependencies
Requires: Redis project (4.1), in-memory Fixed Window as behavioral reference (1.3)

## Parent
Tracks: Epic execution plan
