# [Phase 3] [RateLimit("policy-name")] attribute / endpoint metadata

**Labels:** `phase-3`, `aspnetcore`, `enhancement`

## Phase
**Phase 3: ASP.NET Core Integration** (Weeks 5–6)

## Summary
Allow developers to attach a named policy to controllers/actions/minimal endpoints via `[RateLimit("policy-name")]` (or equivalent endpoint metadata) so limits are declarative and discoverable.

## Tasks
- [ ] Define `RateLimitAttribute` (and/or `IRateLimitMetadata`)
- [ ] Middleware/filter reads endpoint metadata and selects policy
- [ ] Support minimal APIs (endpoint metadata) as well as controllers if feasible
- [ ] Fallback to default policy when attribute absent
- [ ] Unit/integration tests with `WebApplicationFactory` (optional but preferred)

## Acceptance criteria
- [ ] Different endpoints can enforce different policies in one app
- [ ] Missing/invalid policy name fails clearly (startup or first request—document choice)
- [ ] Sample or test demonstrates attribute usage

## Dependencies
Requires: Policy system (3.2), middleware (3.1)

## Parent
Tracks: Epic execution plan
