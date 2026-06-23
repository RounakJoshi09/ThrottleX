# [Phase 6] Dynamic limit adjustments at runtime

**Labels:** `phase-6`, `enhancement`

## Phase
**Phase 6: Advanced Features** (Weeks 14–16)

## Summary
Support adjusting limits at runtime via API/admin surface or programmatic policy mutation—not only config file reload—for incident response and gradual rollout.

## Tasks
- [ ] Define API: e.g. `IRateLimitPolicyAdmin.UpdatePolicy(name, patch)` or options override layer
- [ ] Precedence: runtime override > config > defaults (document)
- [ ] Thread-safe updates; affect subsequent acquires only
- [ ] Optional: minimal admin endpoint (secure/disabled by default)
- [ ] Tests for override apply/revert

## Acceptance criteria
- [ ] Can tighten/loosen a named policy without restart or config file edit
- [ ] Overrides are observable (metrics/logs optional)
- [ ] Security: no unauthenticated public admin by default

## Dependencies
Requires: Hot-reload/policy system (5.1, 3.2)

## Parent
Tracks: Epic execution plan
