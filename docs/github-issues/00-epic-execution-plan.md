# 📋 Epic: ThrottleX Execution Plan & Roadmap

**Labels:** `epic`, `documentation`

## Overview

ThrottleX is a production-ready rate limiting library for .NET 9 with distributed support, dynamic configuration, and real-time observability.

This epic tracks the full execution plan derived from the README roadmap. Individual phase issues and implementation subtasks are linked as child issues.

## Goals

| Goal | Description |
|------|-------------|
| **Easy to adopt** | Simple DI + middleware integration for ASP.NET Core |
| **Algorithm flexibility** | Fixed Window, Token Bucket, Sliding Window (Leaky Bucket later) |
| **Distributed-ready** | In-memory for single instance; Redis for multi-instance |
| **Observable** | Metrics API, success/rejection ratios, per-endpoint stats |
| **Configurable** | Policy-based limits, attributes, hot-reload, time-based rules |

## Architecture (target)

```
ASP.NET Core App
       │
ThrottleX Middleware
       │
  ┌──────────────┐
  │ Policy Resolver    │  (attribute / config / defaults)
  └──────────────┘
       │
  ┌──────────────┐
  │ IRateLimiter        │  (algorithm implementations)
  └──────────────┘
       │
  ┌──────────────┐
  │ IRateLimitStore     │  (InMemory / Redis)
  └──────────────┘
```

### Proposed solution structure

```
ThrottleX.sln
├── ThrottleX/                    # Core library (algorithms, abstractions, DI)
├── ThrottleX.AspNetCore/          # Middleware, attributes, endpoint filters
├── ThrottleX.Redis/               # Redis store + Lua scripts
├── ThrottleX.Tests/               # xUnit unit + integration tests
├── ThrottleX.Benchmarks/          # BenchmarkDotNet
└── samples/
    └── ThrottleX.SampleApi/        # Demo ASP.NET Core API
```

## Execution Phases

### Phase 1: Foundation (Weeks 1–2)
**Goal:** Solid abstractions, first algorithm, test harness

| # | Task | Priority |
|---|------|----------|
| 1.1 | Solution structure & multi-project layout | P0 |
| 1.2 | Core abstractions (`IRateLimiter`, `IRateLimitStore`, result types, options) | P0 |
| 1.3 | Fixed Window Counter (in-memory) | P0 |
| 1.4 | Unit test project + Fixed Window tests | P0 |
| 1.5 | DI extension methods (`AddThrottleX`) | P1 |

### Phase 2: Core Algorithms (Weeks 3–4)
**Goal:** Complete in-memory algorithm suite + performance baseline

| # | Task | Priority |
|---|------|----------|
| 2.1 | Token Bucket (in-memory) | P0 |
| 2.2 | Sliding Window Counter (in-memory) | P0 |
| 2.3 | Comprehensive algorithm test coverage | P0 |
| 2.4 | BenchmarkDotNet project + algorithm benchmarks | P1 |
| 2.5 | Thread-safety & concurrency tests | P1 |

### Phase 3: ASP.NET Core Integration (Weeks 5–6)
**Goal:** Usable middleware for real apps

| # | Task | Priority |
|---|------|----------|
| 3.1 | `ThrottleX.AspNetCore` project + middleware | P0 |
| 3.2 | Policy system (named policies, config binding) | P0 |
| 3.3 | `[RateLimit("policy-name")]` attribute / endpoint metadata | P0 |
| 3.4 | Standard response headers (`X-RateLimit-*`, `Retry-After`) | P1 |
| 3.5 | Sample API project with working examples | P1 |

### Phase 4: Distributed Support (Weeks 7–9)
**Goal:** Multi-instance correctness via Redis

| # | Task | Priority |
|---|------|----------|
| 4.1 | `ThrottleX.Redis` project + connection abstraction | P0 |
| 4.2 | Redis Fixed Window (Lua atomic ops) | P0 |
| 4.3 | Redis Token Bucket (Lua atomic ops) | P0 |
| 4.4 | Redis Sliding Window (Lua atomic ops) | P0 |
| 4.5 | Race condition / multi-instance integration tests | P1 |

### Phase 5: Hot Config & Metrics (Weeks 10–13)
**Goal:** Production operability + first stable release

| # | Task | Priority |
|---|------|----------|
| 5.1 | Hot-reload configuration (`IOptionsMonitor` / change tokens) | P0 |
| 5.2 | Metrics collector + JSON metrics endpoints | P0 |
| 5.3 | Per-endpoint throttle statistics | P1 |
| 5.4 | Documentation, README examples, API docs | P0 |
| 5.5 | **v0.1.0 NuGet release** | P0 |

### Phase 6: Advanced Features (Weeks 14–16)
**Goal:** Power-user capabilities + v1.0.0

| # | Task | Priority |
|---|------|----------|
| 6.1 | Dynamic limit adjustments at runtime | P1 |
| 6.2 | Time-based conditional limits (business hours vs off-hours) | P1 |
| 6.3 | Endpoint grouping / shared buckets | P1 |
| 6.4 | Per-user AND per-endpoint combined limits | P1 |
| 6.5 | Enhanced observability polish | P2 |
| 6.6 | **v1.0.0 release** | P0 |

### Future (v2.0+)

| # | Task | Priority |
|---|------|----------|
| F.1 | Leaky Bucket algorithm | P2 |
| F.2 | Additional distributed store support | P2 |
| F.3 | Web-based monitoring dashboard | P2 |
| F.4 | Grafana dashboard templates | P2 |
| F.5 | Advanced analytics & reporting | P3 |

## Definition of Done (per feature)

- [ ] Implementation complete with XML docs on public APIs
- [ ] Unit tests pass (`dotnet test`)
- [ ] No regressions in existing tests/benchmarks
- [ ] README / sample updated if public API changes
- [ ] Issue linked in PR description

## Tech stack

- **.NET 9** / **C# 13**
- **xUnit** — tests
- **BenchmarkDotNet** — performance
- **StackExchange.Redis** — distributed store
- **MIT** license

## Current state

| Item | Status |
|------|--------|
| Solution / single class library scaffold | ✅ Done |
| Core abstractions | ❌ Not started (placeholder `Class1.cs` only) |
| Algorithms | ❌ Not started |
| ASP.NET Core / Redis / Tests / Samples | ❌ Not started |
| **Overall** | **Week 0 — Foundation setup** |

## How to use this epic

1. Work issues in phase order (Phase 1 → 6); respect dependencies noted on each issue
2. One issue ≈ one focused PR where practical
3. Close child issues as they complete; update this epic checklist periodically
4. Future/v2 issues are intentionally deferred until v1.0.0

See full plan: `docs/EXECUTION_PLAN.md`
