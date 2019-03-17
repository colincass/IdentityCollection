using EPiServer.Personalization.VisitorGroups;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace bmcdavid.Episerver.ClaimsCriteria
{
    /// <summary>
    /// Episerver criteria for claim values
    /// </summary>
    [VisitorGroupCriterion(
        Category = "Technical Criteria",
        Description = "Allows choices around a user's claims",
        DisplayName = "Claim")]
    public class ClaimTypeCriteria : CriterionBase<ClaimTypeCriteriaModel>
    {
        /// <summary>
        /// Determines if criteria matches given principal
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override bool IsMatch(IPrincipal principal, HttpContextBase httpContext)
        {
            if (!(principal is ClaimsPrincipal claimsPrincipal)) { return false; }

            return claimsPrincipal.Claims.Any(CheckClaim);
        }

        private bool CheckClaim(Claim c)
        {
            var claimType = string.IsNullOrWhiteSpace(Model.CustomType) ? Model.Type : Model.CustomType;
            if (c.Type != claimType) { return false; }

            switch (Model.Condition)
            {
                case ClaimCondition.Equals:
                    return Model.Value == c.Value;
                case ClaimCondition.NotEquals:
                    return Model.Value != c.Value;
                case ClaimCondition.Contains:
                    return c.Value.Contains(Model.Value);
                case ClaimCondition.NotContains:
                    return !c.Value.Contains(Model.Value);
                case ClaimCondition.StartsWith:
                    return c.Value.StartsWith(Model.Value);
                case ClaimCondition.NotStartswith:
                    return !c.Value.StartsWith(Model.Value);
                case ClaimCondition.EndsWith:
                    return c.Value.EndsWith(Model.Value);
                case ClaimCondition.NotEndsWith:
                    return !c.Value.EndsWith(Model.Value);
            }

            return false;
        }
    }
}