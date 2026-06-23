# [Phase 3] Sample API project

**Labels:** `phase-3`, `aspnetcore`, `documentation`

## Phase
**Phase 3: ASP.NET Core Integration** (Weeks 5–6)

## Summary
Add `samples/ThrottleX.SampleApi` demonstrating real-world setup: DI, middleware, config policies, attributes, and intentional low limits for easy manual testing.

## Tasks
- [ ] Create minimal Web API sample project
- [ ] `appsettings.json` with multiple policies (e.g. global vs strict)
- [ ] Endpoints: unrestricted (if any), global-limited, attribute-limited
- [ ] README section or `samples/README.md` with run instructions and expected 429 behavior
- [ ] Add sample to solution (optional exclude from default pack)

## Acceptance criteria
- [ ] `dotnet run --project samples/ThrottleX.SampleApi` works
- [ ] Easy to trigger 429 with curl/browser (very low limits in Development)
- [ ] Mirrors README “Quick Start” examples as closely as possible

## Dependencies
Requires: Middleware, policies, attribute, headers (3.1–3.4 preferred)

## Parent
Tracks: Epic execution plan
