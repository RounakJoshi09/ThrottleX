# [Phase 6] Enhanced observability polish

**Labels:** `phase-6`, `observability`, `enhancement`

## Phase
**Phase 6: Advanced Features** (Weeks 14–16)

## Summary
Polish observability beyond basic JSON metrics: richer dimensions, optional OpenTelemetry/event counters, and operational guidance.

## Tasks
- [ ] Review metrics payload for production usefulness (percentiles optional/out of scope if costly)
- [ ] Optional: `System.Diagnostics.Metrics` / EventCounters integration
- [ ] Optional: structured log events on reject (rate-limited, sample rate)
- [ ] Document how to scrape/export metrics (Prometheus via OTEL, etc.—guidance only unless implementing exporter)
- [ ] Sample dashboard notes (Grafana full templates are future F.4)

## Acceptance criteria
- [ ] Operators have at least one path to monitor throttle health without reading logs only
- [ ] Documentation section complete
- [ ] No major performance regression from metrics in benchmarks (sanity check)

## Dependencies
Requires: Metrics API (5.2), per-endpoint stats (5.3)

## Parent
Tracks: Epic execution plan
