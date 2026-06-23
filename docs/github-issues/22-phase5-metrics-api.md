# [Phase 5] Metrics collector + JSON metrics endpoints

**Labels:** `phase-5`, `enhancement`, `observability`

## Phase
**Phase 5: Hot Configuration & Metrics** (Weeks 10–13)

## Summary
Collect runtime throttle metrics (allowed, rejected, optionally latency) and expose JSON endpoint(s) for operators and dashboards.

## Tasks
- [ ] `IThrottleXMetrics` / collector interface; thread-safe counters
- [ ] Middleware/limiter hooks increment metrics on allow/deny
- [ ] Map endpoint(s) e.g. `GET /throttlex/metrics` (configurable path; consider opt-in only)
- [ ] JSON shape: totals, success/rejection ratio, timestamp
- [ ] Security note: protect metrics in production (auth/filter/disable by default—document)
- [ ] Basic tests for counter increments

## Acceptance criteria
- [ ] After traffic, metrics endpoint reflects allows/rejects
- [ ] Opt-in or clearly documented exposure risk
- [ ] Works with sample API

## Dependencies
Requires: ASP.NET Core middleware (3.1)

## Parent
Tracks: Epic execution plan
