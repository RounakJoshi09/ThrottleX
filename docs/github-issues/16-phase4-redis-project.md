# [Phase 4] ThrottleX.Redis project + connection abstraction

**Labels:** `phase-4`, `redis`, `enhancement`

## Phase
**Phase 4: Distributed Support** (Weeks 7–9)

## Summary
Introduce `ThrottleX.Redis` with connection/multiplexer abstraction and DI hooks so distributed algorithms can share a well-configured Redis client.

## Tasks
- [ ] Create `ThrottleX.Redis` project; reference `ThrottleX` core + `StackExchange.Redis`
- [ ] Options: connection string, key prefix, database index, timeouts
- [ ] `AddThrottleXRedis(...)` / integrate with `AddThrottleX` builder pattern
- [ ] Abstract `IConnectionMultiplexer` registration (owned vs external instance)
- [ ] Key naming convention: `{prefix}:{algorithm}:{partitionKey}` (document)
- [ ] Health/dispose considerations documented

## Acceptance criteria
- [ ] Can register Redis-backed store/limiters via DI without touching middleware code
- [ ] Connection options bind from configuration
- [ ] No ASP.NET Core dependency in this package

## Dependencies
Requires: Core abstractions (1.2)

## Parent
Tracks: Epic execution plan
