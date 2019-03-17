using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace bmcdavid.Episerver.ClaimsCriteria
{
    /// <summary>
    /// Provides claim selections for criteria
    /// </summary>
    public class ClaimTypesSelectionFactory : ISelectionFactory
    {
        /// <summary>
        /// Custom claim
        /// </summary>
        public const string CustomClaimType = "http://Custom";

        private static readonly Dictionary<string, string> _claimTypes = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> _customClaimTypes = new Dictionary<string, string>();
        private readonly IClaimTypeSelectorExtension _claimTypeSelectorExtension;

        /// <summary>
        /// UI Constructor
        /// </summary>
        public ClaimTypesSelectionFactory() : this(null) { }

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="claimTypeSelectorExtension"></param>
        public ClaimTypesSelectionFactory(IClaimTypeSelectorExtension claimTypeSelectorExtension)
        {
            _claimTypeSelectorExtension = claimTypeSelectorExtension ?? ServiceLocator.Current.GetInstance<IClaimTypeSelectorExtension>();
        }

        /// <summary>
        /// Gets claims to assign criteria to
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            InitClaims();
            foreach (var key in _customClaimTypes.Keys.OrderBy(k => k))
            {
                yield return new SelectListItem
                {
                    Text = key,
                    Value = _customClaimTypes[key]
                };
            }

            yield return new SelectListItem
            {
                Text = "---Custom Claim------------------",
                Value = CustomClaimType
            };

            foreach (var key in _claimTypes.Keys.OrderBy(k => k))
            {
                yield return new SelectListItem
                {
                    Text = key,
                    Value = _claimTypes[key]
                };
            }
        }

        private void InitClaims()
        {
            if (_claimTypes.Count > 0) { return; }

            var extendedClaims = _claimTypeSelectorExtension.AdditionalClaims;
            foreach (var claim in extendedClaims)
            {
                _customClaimTypes.Add(claim.Key, claim.Value);
            }

            // Default claims
            var claimTypes = typeof(ClaimTypes).GetFields();
            foreach (var info in claimTypes)
            {
                if (info.IsLiteral)
                {
                    _claimTypes.Add(info.Name, info.GetRawConstantValue().ToString());
                }
            }
        }
    }
}