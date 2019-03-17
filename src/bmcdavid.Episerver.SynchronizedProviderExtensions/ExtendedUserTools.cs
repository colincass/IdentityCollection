using bmcdavid.Episerver.ClaimsCriteria;
using bmcdavid.Episerver.SynchronizedProviderExtensions.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// Default extended user tools
    /// </summary>
    public class ExtendedUserTools : IExtendedUserTools
    {
        private readonly ExtendedRoleDbContextFactory _extendedRoleDbContextFactory;
        private readonly IClaimUserTools _claimUserTools;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="extendedRoleDbContextFactory"></param>
        /// <param name="claimUserTools"></param>
        public ExtendedUserTools(ExtendedRoleDbContextFactory extendedRoleDbContextFactory, IClaimUserTools claimUserTools)
        {
            _extendedRoleDbContextFactory = extendedRoleDbContextFactory;
            _claimUserTools = claimUserTools;
        }

        /// <summary>
        /// Adds visitor groups setup as security groups to a user's role claims
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="httpContextBase"></param>
        /// <returns></returns>
        public async Task AddVisitorGroupRolesAsClaimsAsync(ClaimsIdentity identity, HttpContextBase httpContextBase = null) =>
            await _claimUserTools.AddVisitorGroupRolesAsClaimsAsync(identity, httpContextBase);

        /// <summary>
        /// Sets last login, created date, and assigns manual role claims
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="loginDateUtc"></param>
        /// <returns></returns>
        public async Task SetExtendedRolesAsync(ClaimsIdentity identity, DateTime? loginDateUtc = null)
        {
            if (identity == null || string.IsNullOrWhiteSpace(identity.Name)) { throw new ArgumentNullException(nameof(identity)); }

            using (var db = _extendedRoleDbContextFactory.CreateContext())
            {
                var user = await db.TblSynchedUser
                    .Include(u => u.ExtendedUser)
                    .Include(u => u.ExtendedUserRoles)
                    .ThenInclude(u => u.ExtendedRole)
                    .FirstOrDefaultAsync(u => u.UserName == identity.Name)
                    .ConfigureAwait(false);

                var time = loginDateUtc?.ToUniversalTime() ?? DateTime.UtcNow;
                if (user == null) { return; }
                if (user.ExtendedUser == null)
                {
                    user.ExtendedUser = new ExtendedSynchedUser
                    {
                        CreatedUtcDate = time,
                        LastLoginUtcDate = time
                    };
                }
                else
                {
                    user.ExtendedUser.LastLoginUtcDate = time;
                }

                db.TblSynchedUser.Update(user);
                await db.SaveChangesAsync().ConfigureAwait(false);

                foreach (var role in user.ExtendedUserRoles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.ExtendedRole.RoleName));
                }
            }

            return;
        }
    }
}