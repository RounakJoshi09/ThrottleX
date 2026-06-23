# [Phase 2] Token Bucket (in-memory)

**Labels:** `phase-2`, `enhancement`, `algorithm`

## Phase
**Phase 2: Core Algorithms** (Weeks 3–4)

## Summary
Implement the Token Bucket algorithm in-memory. Allows controlled bursts while enforcing a steady refill rate—better UX than Fixed Window for spiky traffic.

## Algorithm behavior
- Bucket holds up to `capacity` tokens
- Tokens refill at a constant rate (`refillRate` per interval, or tokens per second)
- Each request consumes `permitCount` tokens (default 1)
- If insufficient tokens, reject with `RetryAfter` based on time until enough tokens refill
- Thread-safe

## Tasks
- [ ] Implement `TokenBucketRateLimiter` implementing `IRateLimiter`
- [ ] Support `capacity`, `refill rate`, and initial tokens via policy/options
- [ ] Lazy refill on acquire (compute tokens since last update) to avoid background timers per key
- [ ] Partition key isolation
- [ ] Idle key cleanup / memory bounds strategy
- [ ] XML docs explaining burst vs sustained rate

## Acceptance criteria
- [ ] Sustained rate does not exceed refill rate over long windows
- [ ] Burst up to capacity is allowed when bucket is full
- [ ] Rejection returns sensible `RetryAfter` and remaining tokens
- [ ] Thread-safe under parallel acquires

## Dependencies
Requires: Core abstractions
Related: Fixed Window (pattern reference)

## Parent
Tracks: Epic execution plan
