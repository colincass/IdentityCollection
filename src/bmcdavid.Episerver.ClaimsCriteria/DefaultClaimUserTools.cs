using EPiServer.Personalization.VisitorGroups;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;


namespace bmcdavid.Episerver.ClaimsCriteria
{
    /// <summary>
    /// Default IClaimUserTools
    /// </summary>
    public class DefaultClaimUserTools : IClaimUserTools
    {
        private readonly IVisitorGroupRepository _visitorGroupRepository;
        private readonly IVisitorGroupRoleRepository _visitorGroupRoleRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="visitorGroupRepository"></param>
        /// <param name="visitorGroupRoleRepository"></param>
        public DefaultClaimUserTools(IVisitorGroupRepository visitorGroupRepository, IVisitorGroupRoleRepository visitorGroupRoleRepository)
        {
            _visitorGroupRepository = visitorGroupRepository;
            _visitorGroupRoleRepository = visitorGroupRoleRepository;
        }

        /// <summary>
        /// Adds any Episerver visitor groups roles setup as security groups to claims identity
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task AddVisitorGroupRolesAsClaimsAsync(ClaimsIdentity identity, HttpContext httpContext = null)
        {
            if (identity == null || string.IsNullOrWhiteSpace(identity.Name)) { throw new ArgumentNullException(nameof(identity)); }
            //var httpContext = httpContextBase ?? new HttpContextWrapper(httpContext);
            httpContext.User = httpContext.User ?? new ClaimsPrincipal(identity); // its null when user is authenticated...
            var visitorGroups = _visitorGroupRepository.List();
            foreach (var visitorGroup in visitorGroups)
            {
                if (_visitorGroupRoleRepository.TryGetRole(visitorGroup.Name, out var virtualRoleObject) &&
                    virtualRoleObject.IsMatch(httpContext.User, httpContext))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, virtualRoleObject.Name));
                }
            }

            await Task.FromResult(0);
        }
    }
}