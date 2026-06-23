# [Phase 6] Per-user AND per-endpoint combined limits

**Labels:** `phase-6`, `enhancement`

## Phase
**Phase 6: Advanced Features** (Weeks 14–16)

## Summary
Enforce composite limits: e.g. user may call an endpoint only N times, while the endpoint as a whole has a global M—both must pass (AND semantics).

## Tasks
- [ ] Partition strategies: user id (claims/header), endpoint, composite key
- [ ] Policy support for multiple limiters/layers (user policy + endpoint policy)
- [ ] Middleware runs checks in defined order; reject on first failure; return appropriate headers
- [ ] Auth integration notes (anonymous fallback: IP or reject)
- [ ] Tests for both limits independently and combined

## Acceptance criteria
- [ ] User can be limited even when global endpoint quota remains
- [ ] Global endpoint limit applies across users
- [ ] Clear documentation of key extraction and precedence

## Dependencies
Requires: Policy system (3.2), attribute/middleware (3.1, 3.3)

## Parent
Tracks: Epic execution plan
