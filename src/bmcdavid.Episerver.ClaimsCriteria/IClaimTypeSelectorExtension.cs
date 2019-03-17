using System.Collections.Generic;

namespace bmcdavid.Episerver.ClaimsCriteria
{
    /// <summary>
    /// Allows developers ability to add custom claims to criteria claim type dropdown
    /// </summary>
    public interface IClaimTypeSelectorExtension
    {
        /// <summary>
        /// Additional claims to add to claim type dropdown
        /// </summary>
        Dictionary<string, string> AdditionalClaims { get; }
    }
}