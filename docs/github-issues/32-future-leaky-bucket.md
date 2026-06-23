# [Future] Leaky Bucket algorithm

**Labels:** `future`, `algorithm`, `enhancement`

## Phase
**Future (v2.0+)**

## Summary
Add Leaky Bucket algorithm for smooth, constant outflow rate limiting (queue-oriented smoothing vs Token Bucket burstiness).

## Tasks
- [ ] Design in-memory implementation + `IRateLimiter` integration
- [ ] Optional Redis/Lua variant
- [ ] Tests and docs comparing to Token/Sliding/Fixed
- [ ] Policy enum/config support

## Acceptance criteria
- [ ] Behavior matches documented leaky bucket semantics
- [ ] Selectable via policy configuration

## Dependencies
Defer until after v1.0.0 unless prioritized explicitly

## Parent
Tracks: Epic execution plan
