# [Phase 3] Policy system (named policies, config binding)

**Labels:** `phase-3`, `aspnetcore`, `enhancement`

## Phase
**Phase 3: ASP.NET Core Integration** (Weeks 5–6)

## Summary
Implement named rate limit policies with configuration binding so operators can define limits in `appsettings.json` without code changes.

## Example config (preview)
```json
{
  "ThrottleX": {
    "DefaultPolicy": "global",
    "Policies": {
      "global": {
        "Algorithm": "FixedWindow",
        "PermitLimit": 100,
        "Window": "00:01:00"
      },
      "auth": {
        "Algorithm": "TokenBucket",
        "PermitLimit": 10,
        "Window": "00:01:00"
      }
    }
  }
}
```

## Tasks
- [ ] `RateLimitPolicy` / options model complete enough for all three algorithms
- [ ] `IRateLimitPolicyProvider` implementation backed by options
- [ ] Configuration section binding (`ThrottleX`)
- [ ] Middleware selects policy by name (default + override hook)
- [ ] Validation on startup (missing policy name, invalid limits)

## Acceptance criteria
- [ ] Policies load from configuration and are resolvable by name
- [ ] Changing config requires restart for now (hot-reload is Phase 5)
- [ ] Documented in README

## Dependencies
Requires: Core abstractions; middleware (3.1) or parallel if provider is core-only

## Parent
Tracks: Epic execution plan
