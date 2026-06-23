# ThrottleX — Execution Plan

> Derived from [README.md](../README.md) roadmap.  
> **Status:** Week 0 — Foundation setup (scaffold only; `Class1.cs` placeholder).  
> **Blocker:** GitHub repo `RounakJoshi09/ThrottleX` is **archived** (read-only). Unarchive before creating issues via API/UI.

---

## 1. Vision & success criteria

ThrottleX is a **production-ready rate limiting library** for .NET 9 with:

| Pillar | What “done” looks like |
|--------|------------------------|
| **Core algorithms** | Fixed Window, Token Bucket, Sliding Window (in-memory + Redis) |
| **ASP.NET Core** | Middleware, `[RateLimit]` attribute, config-based policies |
| **Distributed** | Redis via StackExchange.Redis + atomic Lua scripts |
| **Observability** | Metrics API, per-endpoint stats, success/rejection ratios |
| **Advanced** | Hot-reload, dynamic limits, time-based policies, combined keys |
| **Releases** | v0.1.0 (end Phase 5), v1.0.0 (end Phase 6) |

**Definition of Done (every feature issue):**

- [ ] Implementation + XML docs on public APIs
- [ ] `dotnet test` passes
- [ ] No regressions in existing tests/benchmarks
- [ ] README/sample updated if public API changes
- [ ] Linked PR closes/tracks the issue

---

## 2. Target architecture

```
┌─────────────────────────────────────────┐
│         ASP.NET Core Application        │
└────────────────┬────────────────────────┘
                 │
         ┌───────▼────────┐
         │  ThrottleX     │
         │  Middleware    │
         └───────┬────────┘
                 │
    ┌────────────┼────────────┐
    │            │            │
┌───▼────┐  ┌───▼────┐  ┌───▼────┐
│ Token  │  │ Fixed  │  │Sliding │
│ Bucket │  │ Window │  │ Window │
└───┬────┘  └───┬────┘  └───┬────┘
    │            │            │
    └────────────┼────────────┘
                 │
         ┌───────▼────────┐
         │  Storage Layer │
         │ (In-Memory/    │
         │   Redis)       │
         └────────────────┘
```

### Layering

| Layer | Responsibility |
|-------|----------------|
| **Abstractions** | `IRateLimiter`, `IRateLimitStore`, `IRateLimitPolicyProvider`, result/lease types |
| **Algorithms** | Fixed Window, Token Bucket, Sliding Window |
| **Storage** | In-memory (core); Redis (separate package) |
| **Hosting** | `AddThrottleX()` DI (core, no ASP.NET dependency) |
| **ASP.NET Core** | Middleware, attributes, headers, endpoint metadata |
| **Observability** | Metrics collector + JSON endpoints |
| **Config** | Named policies, `IOptions`/`IOptionsMonitor`, hot-reload |

### Proposed solution structure

```
ThrottleX.sln
├── ThrottleX/                     # Core: abstractions, algorithms, in-memory store, DI
├── ThrottleX.AspNetCore/          # Middleware, attributes, endpoint filters
├── ThrottleX.Redis/               # Redis store + Lua scripts
├── ThrottleX.Tests/               # xUnit
├── ThrottleX.Benchmarks/          # BenchmarkDotNet
└── samples/
    └── ThrottleX.SampleApi/       # Demo API
```

---

## 3. Phase overview

| Phase | Focus | Outcome |
|-------|--------|---------|
| **1 — Foundation** | Structure, abstractions, Fixed Window, tests, DI | First working limiter in-process |
| **2 — Core algorithms** | Token Bucket, Sliding Window, benchmarks, concurrency | Complete in-memory suite |
| **3 — ASP.NET Core** | Middleware, policies, attributes, sample | Usable in real web apps |
| **4 — Distributed** | Redis + Lua, multi-instance correctness | Horizontal scale |
| **5 — Ops & release** | Hot-reload, metrics, docs, **v0.1.0** | First stable NuGet |
| **6 — Advanced** | Dynamic/time-based/combined limits, **v1.0.0** | Power-user feature set |
| **Future (v2+)** | Leaky bucket, more stores, Web UI, Grafana | Deferred |

**Recommended execution order:** strict phase order. Within a phase, respect issue dependencies (abstractions → algorithm → tests → DI/middleware).

---

## 4. Phase 1: Foundation

**Goal:** Solid contracts, first algorithm, test harness.

| ID | Issue title | Priority | Depends on |
|----|-------------|----------|------------|
| 1.1 | Solution structure & multi-project layout | P0 | — |
| 1.2 | Core abstractions and interfaces | P0 | 1.1 (soft) |
| 1.3 | Fixed Window Counter (in-memory) | P0 | 1.2 |
| 1.4 | Unit test project + Fixed Window tests | P0 | 1.3 |
| 1.5 | DI extension methods (`AddThrottleX`) | P1 | 1.2, 1.3 |

**Exit criteria:**

- Solution builds; `dotnet test` green for Fixed Window
- Consumer can `services.AddThrottleX()` and acquire/reject permits programmatically
- Placeholder `Class1.cs` removed

---

## 5. Phase 2: Core algorithms

**Goal:** Full in-memory algorithm suite + performance baseline.

| ID | Issue title | Priority | Depends on |
|----|-------------|----------|------------|
| 2.1 | Token Bucket (in-memory) | P0 | 1.2 |
| 2.2 | Sliding Window Counter (in-memory) | P0 | 1.2 |
| 2.3 | Comprehensive algorithm test coverage | P0 | 2.1, 2.2 |
| 2.4 | BenchmarkDotNet project + benchmarks | P1 | 1.3, 2.1, 2.2 |
| 2.5 | Thread-safety & concurrency tests | P1 | 2.3 |

**Exit criteria:**

- All three algorithms implement `IRateLimiter` with consistent result semantics
- Benchmarks runnable via `dotnet run --project ThrottleX.Benchmarks -c Release`
- Concurrency tests document guarantees (strict vs eventual tolerance)

---

## 6. Phase 3: ASP.NET Core integration

**Goal:** Drop-in throttling for web APIs.

| ID | Issue title | Priority | Depends on |
|----|-------------|----------|------------|
| 3.1 | `ThrottleX.AspNetCore` + middleware | P0 | 1.5, Phase 2 (min: Fixed Window) |
| 3.2 | Policy system (named policies, config binding) | P0 | 1.2, 3.1 |
| 3.3 | `[RateLimit("policy-name")]` attribute / metadata | P0 | 3.2 |
| 3.4 | Standard response headers (`X-RateLimit-*`, `Retry-After`) | P1 | 3.1 |
| 3.5 | Sample API project | P1 | 3.1–3.4 |

**Exit criteria:**

- Middleware returns **429** with correct headers when limited
- Policies selectable via attribute and `appsettings.json`
- Sample API demonstrates global + per-endpoint policies

---

## 7. Phase 4: Distributed support

**Goal:** Correct multi-instance limiting via Redis.

| ID | Issue title | Priority | Depends on |
|----|-------------|----------|------------|
| 4.1 | `ThrottleX.Redis` + connection abstraction | P0 | 1.2 |
| 4.2 | Redis Fixed Window (Lua) | P0 | 4.1, 1.3 |
| 4.3 | Redis Token Bucket (Lua) | P0 | 4.1, 2.1 |
| 4.4 | Redis Sliding Window (Lua) | P0 | 4.1, 2.2 |
| 4.5 | Multi-instance / race integration tests | P1 | 4.2–4.4 |

**Exit criteria:**

- Atomic operations (Lua) prevent over-admission under concurrent multi-instance load (validated by tests or documented limits)
- `AddThrottleX().UseRedis(...)` (or equivalent) swaps storage without changing middleware API

---

## 8. Phase 5: Hot configuration & metrics

**Goal:** Production operability + first public package.

| ID | Issue title | Priority | Depends on |
|----|-------------|----------|------------|
| 5.1 | Hot-reload configuration | P0 | 3.2 |
| 5.2 | Metrics collector + JSON endpoints | P0 | 3.1 |
| 5.3 | Per-endpoint throttle statistics | P1 | 5.2 |
| 5.4 | Documentation & examples polish | P0 | Phases 1–4 largely done |
| 5.5 | **v0.1.0 NuGet release** | P0 | 5.1, 5.2, 5.4 |

**Exit criteria:**

- Policy changes apply without process restart (within configured latency)
- Metrics endpoint(s) expose counts/ratios suitable for basic monitoring
- Package published (or release workflow ready); version `0.1.0` tagged

---

## 9. Phase 6: Advanced features

**Goal:** Power-user controls + v1.0.0.

| ID | Issue title | Priority | Depends on |
|----|-------------|----------|------------|
| 6.1 | Dynamic limit adjustments at runtime | P1 | 5.1 |
| 6.2 | Time-based conditional limits | P1 | 3.2 |
| 6.3 | Endpoint grouping / shared buckets | P1 | 3.2 |
| 6.4 | Per-user AND per-endpoint combined limits | P1 | 3.2, partition keys |
| 6.5 | Enhanced observability polish | P2 | 5.2, 5.3 |
| 6.6 | **v1.0.0 release** | P0 | 6.1–6.4 (P1 items), stability |

**Exit criteria:**

- Advanced policies expressible via config without custom code for common cases
- Breaking-change policy documented for 1.x
- `1.0.0` tagged and published

---

## 10. Future (v2.0+)

| ID | Issue title | Priority |
|----|-------------|----------|
| F.1 | Leaky Bucket algorithm | P2 |
| F.2 | Additional distributed stores (beyond Redis) | P2 |
| F.3 | Web-based monitoring dashboard | P2 |
| F.4 | Grafana dashboard templates | P2 |
| F.5 | Advanced analytics & reporting | P3 |

Do not prioritize these ahead of v1.0.0 unless explicitly requested.

---

## 11. Suggested sprint order (first 4 weeks)

| Week | Focus | Issues |
|------|--------|--------|
| **1** | Structure + abstractions + Fixed Window | 1.1, 1.2, 1.3 |
| **2** | Tests + DI; start Token Bucket | 1.4, 1.5, 2.1 |
| **3** | Sliding Window + full algorithm tests | 2.2, 2.3 |
| **4** | Benchmarks + concurrency; start middleware | 2.4, 2.5, 3.1 (spike) |

---

## 12. Risk register

| Risk | Mitigation |
|------|------------|
| Abstraction wrong for Redis/Lua | Spike Redis Fixed Window early (end Phase 2 / start Phase 4); keep store interface thin |
| Clock skew / non-deterministic tests | Use `TimeProvider` injection on .NET 9 |
| Over-scoping Phase 1 | Ship Fixed Window only; defer Token/Sliding to Phase 2 |
| Repo archived — can't track issues on GitHub | Unarchive repo; run `scripts/create-github-issues.ps1` or create issues manually from `docs/github-issues/` |
| Window-boundary burst (Fixed Window) | Document; offer Sliding Window as smoother alternative |

---

## 13. Creating GitHub issues

1. **Unarchive** the repository on GitHub: Settings → Danger Zone → Unarchive.
2. Optionally create labels: `epic`, `phase-1` … `phase-6`, `future`, `foundation`, `algorithm`, `aspnetcore`, `redis`, `testing`, `enhancement`, `chore`, `documentation`, `release`.
3. Either:
   - Run [`scripts/create-github-issues.ps1`](../scripts/create-github-issues.ps1) (requires [GitHub CLI](https://cli.github.com/) `gh auth login`), or
   - Manually open issues using bodies in [`docs/github-issues/`](github-issues/).
4. Link child issues back to the epic issue once created.

---

## 14. Current repo state (audit)

| Item | Status |
|------|--------|
| `ThrottleX.sln` + `ThrottleX/` class library (`net9.0`) | ✅ Present |
| NuGet metadata on `ThrottleX.csproj` | ✅ Present |
| `README.md` vision/roadmap | ✅ Present |
| Core abstractions / algorithms | ❌ Not started (`Class1.cs` only) |
| Tests / AspNetCore / Redis / Benchmarks / Samples | ❌ Not started |
| GitHub issues | ❌ Blocked (archived) or empty |

**Immediate next action after unarchiving:** Create issues (section 13), then implement **1.1 → 1.2 → 1.3** and open the first implementation PR.
