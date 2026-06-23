# [Phase 6] Endpoint grouping / shared buckets

**Labels:** `phase-6`, `enhancement`

## Phase
**Phase 6: Advanced Features** (Weeks 14–16)

## Summary
Allow multiple endpoints to share one rate limit bucket (group quota) so related routes consume a common budget (e.g. all `/api/search/*` share 100 req/min).

## Tasks
- [ ] Config/attribute: `Group = "search"` or policy-level group key
- [ ] Partition key includes group id (not only endpoint path)
- [ ] Document interaction with per-endpoint limits (group only vs group + endpoint—scope this issue to group shared bucket first)
- [ ] Tests: two endpoints in same group increment same counter
- [ ] Sample/config example

## Acceptance criteria
- [ ] Traffic to endpoint A reduces remaining quota for endpoint B in the same group
- [ ] Endpoints in different groups remain isolated
- [ ] Documented configuration patterns

## Dependencies
Requires: Policy system (3.2), middleware partition key resolution

## Parent
Tracks: Epic execution plan
