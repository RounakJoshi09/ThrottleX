# [Phase 1] Solution structure & multi-project layout

**Labels:** `phase-1`, `foundation`, `chore`

## Phase
**Phase 1: Foundation** (Weeks 1–2)

## Summary
Reorganize the repo from a single placeholder project into the target multi-project solution layout so algorithms, ASP.NET Core integration, Redis, tests, benchmarks, and samples can evolve independently.

## Tasks
- [ ] Expand `ThrottleX.sln` with planned projects (stub or full as needed)
- [ ] Keep `ThrottleX` as the **core** library (algorithms, abstractions, DI)
- [ ] Add empty/stub projects (or create incrementally as later issues land):
  - `ThrottleX.AspNetCore`
  - `ThrottleX.Redis`
  - `ThrottleX.Tests`
  - `ThrottleX.Benchmarks`
  - `samples/ThrottleX.SampleApi`
- [ ] Align `ThrottleX.csproj` package metadata with solution structure (README pack path, etc.)
- [ ] Ensure `dotnet build` / `dotnet restore` succeed on the full solution
- [ ] Remove or replace placeholder `Class1.cs` once abstractions issue lands

## Acceptance criteria
- [ ] Solution builds cleanly on .NET 9
- [ ] Project references are correct (Tests → Core, AspNetCore → Core, Redis → Core, Sample → AspNetCore)
- [ ] Directory structure matches the epic architecture section

## Dependencies
None — start here (or in parallel with core abstractions).

## Parent
Tracks: Epic execution plan
