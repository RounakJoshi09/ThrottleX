# [Phase 3] Standard response headers (X-RateLimit-*, Retry-After)

**Labels:** `phase-3`, `aspnetcore`, `enhancement`

## Phase
**Phase 3: ASP.NET Core Integration** (Weeks 5–6)

## Summary
Emit standard/familiar rate limit headers on limited and optionally successful responses so clients and API gateways can react predictably.

## Headers (target)
| Header | Purpose |
|--------|---------|
| `X-RateLimit-Limit` | Policy permit limit |
| `X-RateLimit-Remaining` | Remaining permits in window/bucket |
| `X-RateLimit-Reset` | Unix time or seconds until reset (pick one; document) |
| `Retry-After` | Seconds until client should retry (on 429) |

## Tasks
- [ ] Centralize header names in `RateLimitHeaders` (core or aspnetcore)
- [ ] Write headers on reject (required); optionally on success
- [ ] Align values with `RateLimitLease` from core limiter
- [ ] Tests asserting headers on 429 responses
- [ ] Document header semantics in README

## Acceptance criteria
- [ ] 429 responses include `Retry-After` when known
- [ ] Limit/remaining/reset headers present and consistent with policy
- [ ] Behavior documented (success path on/off)

## Dependencies
Requires: Middleware (3.1)

## Parent
Tracks: Epic execution plan
