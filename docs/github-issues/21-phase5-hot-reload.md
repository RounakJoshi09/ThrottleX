# [Phase 5] Hot-reload configuration

**Labels:** `phase-5`, `enhancement`

## Phase
**Phase 5: Hot Configuration & Metrics** (Weeks 10–13)

## Summary
Allow policy/limit changes via configuration reload (`IOptionsMonitor` / change tokens) without restarting the application process.

## Tasks
- [ ] Hook policies to `IOptionsMonitor<ThrottleXOptions>` (or equivalent)
- [ ] On change: update policy provider; define behavior for in-flight requests (use new policy on next acquire)
- [ ] Document which settings are hot-reloadable vs require restart (e.g. Redis connection may not be)
- [ ] Tests: simulate options change and verify new limits apply
- [ ] Sample/appsettings note for `reloadOnChange: true`

## Acceptance criteria
- [ ] Changing a policy limit in config (with reload) affects subsequent requests without restart
- [ ] No crashes/races during options swap (thread-safe provider)
- [ ] Documented limitations

## Dependencies
Requires: Policy system (3.2)

## Parent
Tracks: Epic execution plan
