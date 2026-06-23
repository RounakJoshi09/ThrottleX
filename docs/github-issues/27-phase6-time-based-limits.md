# [Phase 6] Time-based conditional limits

**Labels:** `phase-6`, `enhancement`

## Phase
**Phase 6: Advanced Features** (Weeks 14–16)

## Summary
Apply different limits based on time windows (e.g. business hours vs off-hours, weekends), using configurable schedules and time zones.

## Tasks
- [ ] Config model: schedule segments with start/end, days of week, time zone, linked policy or limit override
- [ ] Resolver picks active segment at request time (`TimeProvider`)
- [ ] Fallback policy when no segment matches
- [ ] Tests with fixed `TimeProvider` across boundaries
- [ ] Document examples in README/docs

## Acceptance criteria
- [ ] Same endpoint enforces different limits at different configured times
- [ ] Time zone behavior is explicit and tested
- [ ] Misconfigured schedules fail validation with clear errors

## Dependencies
Requires: Policy system (3.2)

## Parent
Tracks: Epic execution plan
