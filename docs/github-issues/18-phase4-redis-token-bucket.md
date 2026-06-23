# [Phase 4] Redis Token Bucket (Lua atomic ops)

**Labels:** `phase-4`, `redis`, `enhancement`, `algorithm`

## Phase
**Phase 4: Distributed Support** (Weeks 7–9)

## Summary
Implement distributed Token Bucket with atomic Lua refill + consume so concurrent instances share one bucket state.

## Tasks
- [ ] Lua script: compute refill from timestamps, consume tokens, write back state atomically
- [ ] Store fields: tokens, last refill timestamp (or equivalent)
- [ ] `RedisTokenBucketRateLimiter` + lease mapping
- [ ] Error handling policy consistent with other Redis limiters
- [ ] Tests with Redis

## Acceptance criteria
- [ ] Burst/capacity semantics match in-memory Token Bucket design
- [ ] No lost updates under concurrent multi-instance acquires (validated by tests or stress script)
- [ ] TTL/cleanup strategy prevents orphaned keys when appropriate

## Dependencies
Requires: Redis project (4.1), Token Bucket in-memory (2.1)

## Parent
Tracks: Epic execution plan
