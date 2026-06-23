# [Future] Web-based monitoring dashboard

**Labels:** `future`, `observability`, `enhancement`

## Phase
**Future (v2.0+)**

## Summary
Build or host a lightweight web UI for live throttle monitoring (endpoints, allow/reject rates, top limited routes)—v2.0 vision item from README.

## Tasks
- [ ] Scope: embedded static UI vs separate app
- [ ] Consume metrics API; optional auth
- [ ] UX for policy list / recent rejects
- [ ] Security model (must not expose by default unauthenticated)

## Acceptance criteria
- [ ] Operator can view live throttle health in a browser
- [ ] Documented enablement steps

## Dependencies
Requires: Metrics + per-endpoint stats (Phase 5); defer until after v1.0.0

## Parent
Tracks: Epic execution plan
