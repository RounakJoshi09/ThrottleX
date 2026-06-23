# [Phase 2] BenchmarkDotNet project + algorithm benchmarks

**Labels:** `phase-2`, `testing`, `chore`

## Phase
**Phase 2: Core Algorithms** (Weeks 3–4)

## Summary
Add `ThrottleX.Benchmarks` with BenchmarkDotNet to establish performance baselines for in-memory algorithms (single-threaded and parallel acquires).

## Tasks
- [ ] Create `ThrottleX.Benchmarks` project (Release-oriented)
- [ ] Reference core `ThrottleX`
- [ ] Benchmarks per algorithm: acquire success path, reject path, many partition keys
- [ ] Optional: parallel/throughput scenarios
- [ ] Document how to run in README (`dotnet run --project ThrottleX.Benchmarks -c Release`)
- [ ] Store baseline notes in issue/PR or `docs/benchmarks/` (optional, not required for merge)

## Acceptance criteria
- [ ] Benchmarks run successfully in Release
- [ ] At least one benchmark class per implemented algorithm
- [ ] Results are reproducible enough to compare before/after changes

## Dependencies
Requires: Fixed Window, Token Bucket, Sliding Window (or benchmark only implemented ones incrementally)

## Parent
Tracks: Epic execution plan
