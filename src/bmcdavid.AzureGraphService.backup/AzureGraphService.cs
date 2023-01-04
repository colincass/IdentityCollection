using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace bmcdavid.AzureGraphService
{
    /// <summary>
    /// Azure Graph Service
    /// </summary>

    public class AzureGraphService
    {
        private readonly IAzureGraphServiceOptions _azureGraphServiceOptions;
        private readonly string _claimIssuer;
        private readonly Uri _servicePointUri;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="azureGraphServiceOptions"></param>
        public AzureGraphService(IAzureGraphServiceOptions azureGraphServiceOptions)
        {
            _azureGraphServiceOptions = azureGraphServiceOptions;
            _claimIssuer = nameof(AzureGraphService);
            _servicePointUri = new Uri(_azureGraphServiceOptions.GraphUrl);
        }

        /// <summary>
        /// Adds claims to identity
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public virtual async Task AddGroupsAsRoleClaimsAsync(ClaimsIdentity identity)
        {
            var currentUserObjectId = identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var tentantId = identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            var groups = await GetUserGroupsAsync(currentUserObjectId, tentantId);

            foreach (var group in groups)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, group.DisplayName, ClaimValueTypes.String, _claimIssuer));
            }
        }

        /// <summary>
        /// Gets nested/transient groups for a user object Id
        /// </summary>
        /// <param name="userObjectId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Group>> GetUserGroupsAsync(string userObjectId, string tenantId = null)
        {
            var adClient = CreateClient(tenantId: tenantId);
            var userFetcher = adClient.Users.GetByObjectId(userObjectId);
            var userGroupIds = await userFetcher.GetMemberGroupsAsync(securityEnabledOnly: false).ConfigureAwait(continueOnCapturedContext: false);
            var userGroupIdList = userGroupIds.ToList();
            var userGroups = await adClient.GetObjectsByObjectIdsAsync(userGroupIdList, new List<string> { "group" }).ConfigureAwait(continueOnCapturedContext: false);

            return new List<Group>(userGroups.OfType<Group>());
        }

        /// <summary>
        /// Finds groups
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<string>> FindGroupsAsync(string prefix = "")
        {
            List<string> adRoles = new List<string>();
            var adClient = CreateClient();
            var groupQuery = adClient.Groups.OfType<IGroup>();

            if (!string.IsNullOrWhiteSpace(prefix))
            {
                groupQuery = groupQuery.Where(g => g.DisplayName.StartsWith(prefix));
            }

            var groups = await groupQuery.ExecuteAsync().ConfigureAwait(continueOnCapturedContext: false);

            do
            {
                var paged = groups.CurrentPage.ToList();
                foreach (var group in paged)
                {
                    adRoles.Add(group.DisplayName);
                }

                groups = await groups.GetNextPageAsync().ConfigureAwait(continueOnCapturedContext: false);

            } while (groups?.MorePagesAvailable == true);

            return adRoles;
        }

        private async Task<string> AcquireTokenAsyncForApplication()
        {
            var authenticationContext = new AuthenticationContext(string.Format("https://login.windows.net/{0}", _azureGraphServiceOptions.TenantName), false);
            var clientCred = new ClientCredential(_azureGraphServiceOptions.ClientId, _azureGraphServiceOptions.ClientSecret);
            var authenticationResult = await authenticationContext.AcquireTokenAsync(_azureGraphServiceOptions.GraphUrl, clientCred);

            return authenticationResult.AccessToken;
        }

        private ActiveDirectoryClient CreateClient(string servicePointUri = null, string tenantId = null)
        {
            var serviceRoot = new Uri(_servicePointUri, tenantId ?? _azureGraphServiceOptions.TenantId);

            return new ActiveDirectoryClient(serviceRoot, async () => await AcquireTokenAsyncForApplication());
        }
    }
}
