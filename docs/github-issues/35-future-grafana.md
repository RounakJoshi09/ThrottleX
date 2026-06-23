# [Future] Grafana dashboard templates

**Labels:** `future`, `observability`, `documentation`

## Phase
**Future (v2.0+)**

## Summary
Ship Grafana dashboard JSON templates for ThrottleX metrics (via Prometheus/OpenTelemetry export path).

## Tasks
- [ ] Define required metrics/labels for useful panels
- [ ] Provide dashboard JSON under `docs/grafana/` or `deploy/grafana/`
- [ ] Document datasource assumptions and export setup
- [ ] Sample panels: RPS allowed/rejected, top endpoints, reject ratio

## Acceptance criteria
- [ ] Dashboard imports into Grafana with minimal edits
- [ ] Docs explain metric source requirements

## Dependencies
Prefer after observability polish (6.5) / OTEL metrics; defer until after v1.0.0

## Parent
Tracks: Epic execution plan
