# [Phase 3] ThrottleX.AspNetCore project + middleware

**Labels:** `phase-3`, `aspnetcore`, `enhancement`

## Phase
**Phase 3: ASP.NET Core Integration** (Weeks 5–6)

## Summary
Create `ThrottleX.AspNetCore` and implement middleware that enforces rate limits on incoming HTTP requests using the core `IRateLimiter` / policy system.

## Tasks
- [ ] Create `ThrottleX.AspNetCore` project targeting `net9.0` with ASP.NET Core shared framework reference
- [ ] Implement `ThrottleXMiddleware` (or endpoint filter approach—document choice)
- [ ] Resolve partition key (default: client IP; extensible later)
- [ ] Resolve policy (global default first; per-endpoint in follow-up issues)
- [ ] On reject: return **429 Too Many Requests** with problem details or simple body
- [ ] `UseThrottleX()` / `AddThrottleXAspNetCore()` extension methods
- [ ] Wire into solution + project references

## Acceptance criteria
- [ ] Requests over limit receive 429; under limit pass through unchanged
- [ ] No circular dependency: AspNetCore → Core only
- [ ] Works with minimal setup in a blank Web API

## Dependencies
Requires: DI extensions (1.5), at least Fixed Window (1.3); Phase 2 algorithms recommended

## Parent
Tracks: Epic execution plan
