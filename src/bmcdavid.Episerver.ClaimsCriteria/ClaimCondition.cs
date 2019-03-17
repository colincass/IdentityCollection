#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace bmcdavid.Episerver.ClaimsCriteria
{
    /// <summary>
    /// Criteria claim conditions
    /// </summary>
    public enum ClaimCondition
    {
        Equals,
        NotEquals,
        Contains,
        NotContains,
        StartsWith,
        NotStartswith,
        EndsWith,
        NotEndsWith
    }
}