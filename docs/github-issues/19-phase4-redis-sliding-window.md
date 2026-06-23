# [Phase 4] Redis Sliding Window (Lua atomic ops)

**Labels:** `phase-4`, `redis`, `enhancement`, `algorithm`

## Phase
**Phase 4: Distributed Support** (Weeks 7–9)

## Summary
Implement distributed Sliding Window using Redis (sorted set of timestamps or weighted two-window counters via Lua—match in-memory variant choice).

## Tasks
- [ ] Choose Redis data structure aligned with in-memory Sliding Window design
- [ ] Lua script for atomic add/trim/count (or increment weighted windows)
- [ ] `RedisSlidingWindowRateLimiter` + lease mapping
- [ ] Memory/TTL strategy (trim old entries; key expiry)
- [ ] Tests with Redis

## Acceptance criteria
- [ ] Multi-instance admission stays within limit
- [ ] Boundary smoothing behavior documented vs Fixed Window
- [ ] Operational cost (Redis memory/commands) noted in docs

## Dependencies
Requires: Redis project (4.1), Sliding Window in-memory (2.2)

## Parent
Tracks: Epic execution plan
