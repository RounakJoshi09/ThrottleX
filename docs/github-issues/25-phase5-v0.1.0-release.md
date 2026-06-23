# [Phase 5] v0.1.0 NuGet release

**Labels:** `phase-5`, `release`

## Phase
**Phase 5: Hot Configuration & Metrics** (Weeks 10–13)

## Summary
Ship the first public NuGet packages for ThrottleX (core, AspNetCore, Redis as applicable) at version **0.1.0**.

## Tasks
- [ ] Align package versions across projects (`0.1.0`)
- [ ] Ensure README packs correctly; symbols/source link if configured
- [ ] `dotnet pack` produces valid packages locally
- [ ] Tag `v0.1.0` in git
- [ ] Publish to NuGet.org (or document publish steps / CI workflow)
- [ ] GitHub Release notes summarizing features & known limitations
- [ ] Update README install instructions with real package IDs

## Acceptance criteria
- [ ] Consumers can `dotnet add package ThrottleX` (and related packages) successfully
- [ ] Release notes list supported algorithms/features and “not yet” items
- [ ] SemVer: 0.1.0 clearly pre-1.0 (API may evolve)

## Dependencies
Requires: Hot-reload (5.1), metrics (5.2), documentation (5.4); core features from Phases 1–4

## Parent
Tracks: Epic execution plan
