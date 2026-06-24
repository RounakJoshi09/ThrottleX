namespace ThrottleX.Results;

/// <summary>
/// Well-known HTTP header names for rate-limit responses (ASP.NET Core middleware will set these).
/// </summary>
public static class RateLimitHeaders
{
    /// <summary>Maximum permits allowed in the current window/bucket.</summary>
    public const string Limit = "X-RateLimit-Limit";

    /// <summary>Remaining permits/tokens after the request was evaluated.</summary>
    public const string Remaining = "X-RateLimit-Remaining";

    /// <summary>Unix epoch seconds when the current window resets (fixed/sliding window).</summary>
    public const string Reset = "X-RateLimit-Reset";

    /// <summary>Seconds until the client should retry (also standard <c>Retry-After</c>).</summary>
    public const string RetryAfter = "Retry-After";
}
