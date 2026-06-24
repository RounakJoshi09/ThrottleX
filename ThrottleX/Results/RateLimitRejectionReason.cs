namespace ThrottleX.Results;

/// <summary>
/// Why a rate-limit acquisition failed, when applicable.
/// </summary>
public enum RateLimitRejectionReason
{
    /// <summary>Request was allowed.</summary>
    None = 0,

    /// <summary>The partition exceeded its configured permit/token limit.</summary>
    LimitExceeded = 1,

    /// <summary>The request asked for more permits than the policy allows in a single window/capacity.</summary>
    PermitCountExceedsLimit = 2
}
