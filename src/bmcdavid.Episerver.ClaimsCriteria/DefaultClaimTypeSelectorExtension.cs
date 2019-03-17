using System.Collections.Generic;

namespace bmcdavid.Episerver.ClaimsCriteria
{
    /// <summary>
    /// Default claim type selector extension, provides no extra claims
    /// </summary>
    public class DefaultClaimTypeSelectorExtension : IClaimTypeSelectorExtension
    {
        private static readonly Dictionary<string, string> _defaults = new Dictionary<string, string>();

        /// <summary>
        /// Custom claims to add to criteria dropdown
        /// </summary>
        public Dictionary<string, string> AdditionalClaims => _defaults;
    }
}