# [Future] Additional distributed stores (beyond Redis)

**Labels:** `future`, `enhancement`

## Phase
**Future (v2.0+)**

## Summary
Support alternative distributed backends (e.g. NCache, SQL, Azure Cache for Redis abstractions, or other) behind `IRateLimitStore` without changing middleware API.

## Tasks
- [ ] Evaluate candidates and community demand
- [ ] Define store capability matrix (atomic increment, TTL, Lua-equivalent)
- [ ] Implement first non-Redis store as separate package
- [ ] Docs + tests

## Acceptance criteria
- [ ] At least one additional store package or documented extension guide
- [ ] Core/middleware unchanged for consumers

## Dependencies
Defer until after v1.0.0; Redis path should remain primary

## Parent
Tracks: Epic execution plan
