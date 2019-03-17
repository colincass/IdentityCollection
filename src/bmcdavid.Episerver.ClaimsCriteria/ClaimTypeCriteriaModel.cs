using EPiServer.Personalization.VisitorGroups;
using EPiServer.Web.Mvc.VisitorGroups;
using System.ComponentModel.DataAnnotations;

namespace bmcdavid.Episerver.ClaimsCriteria
{
    /// <summary>
    /// Claim criteria model
    /// </summary>
    public class ClaimTypeCriteriaModel : CriterionModelBase, IValidateCriterionModel
    {
        /// <summary>
        /// Claim type
        /// </summary>
        [Required]
        [DojoWidget(
            WidgetType = "dijit.form.FilteringSelect",
            SelectionFactoryType = typeof(ClaimTypesSelectionFactory))]
        public string Type { get; set; }

        /// <summary>
        /// Custom claim type
        /// </summary>
        public string CustomType { get; set; }

        /// <summary>
        /// Condition
        /// </summary>
        [Required]
        [DojoWidget(
            WidgetType = "dijit.form.FilteringSelect",
            SelectionFactoryType = typeof(EnumSelectionFactory))]
        public ClaimCondition Condition { get; set; }

        /// <summary>
        /// Value in claim
        /// </summary>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// Copy
        /// </summary>
        /// <returns></returns>
        public override ICriterionModel Copy() { return ShallowCopy(); }

        /// <summary>
        /// Validates visitor group
        /// </summary>
        /// <param name="currentGroup"></param>
        /// <returns></returns>
        public CriterionValidationResult Validate(VisitorGroup currentGroup)
        {
            if (Type == ClaimTypesSelectionFactory.CustomClaimType)
            {
                if (string.IsNullOrWhiteSpace(CustomType))
                {
                    return new CriterionValidationResult(false, $"{nameof(CustomType)} must be entered when choosing Custom type option!");
                }
            }

            return new CriterionValidationResult(true);
        }
    }
}