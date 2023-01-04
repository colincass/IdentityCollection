using bmcdavid.Episerver.SynchronizedProviderExtensions.Entities;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.Shell.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// Episerver UI Role provider for using extended role table
    /// </summary>
    public class ExtendedRoleProvider : UIRoleProvider, IDisposable
    {
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(ExtendedRoleProvider));
        private readonly ExtendedRoleDbContextFactory _episerverDbContextFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="episerverDbContextFactory"></param>
        public ExtendedRoleProvider(ExtendedRoleDbContextFactory episerverDbContextFactory)
        {
            _episerverDbContextFactory = episerverDbContextFactory ?? throw new ArgumentNullException(nameof(episerverDbContextFactory));
        }

        /// <summary>
        /// Enabled
        /// </summary>
        public override bool Enabled { get; set; } = true;

        /// <summary>
        /// Name
        /// </summary>
        public override string Name => nameof(ExtendedRoleProvider);

        public override IAsyncEnumerable<IUIRole> GetAllRolesAsync()
        {
            throw new NotImplementedException();
        }

        #region obsoletemethods
        /// <summary>
        /// Adds username to given roles
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleNames"></param>
        public override void AddUserToRoles(string username, IEnumerable<string> roleNames)
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                var rolesList = roleNames.ToList();
                var roles = ctx.ExtendedRoles.Where(x => rolesList.Contains(x.RoleName)).ToList();
                var user = ctx.TblSynchedUser
                    .Include(p => p.ExtendedUserRoles)
                    .ThenInclude(p => p.ExtendedRole)
                    .FirstOrDefault(u => u.UserName == username);

                if (user == null) { return; }

                foreach (var role in roles)
                {
                    if (user.ExtendedUserRoles.Any(x => x.ExtendedRoleId == role.Id)) { continue; }

                    user.ExtendedUserRoles.Add(new ExtendedRoleSynchedUser { ExtendedRoleId = role.Id, SynchedUserId = user.PkId });
                }

                ctx.TblSynchedUser.Update(user);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <param name="newRoleName"></param>
        public override void CreateRole(string newRoleName)
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                ctx.ExtendedRoles.Add(new ExtendedRole { RoleName = newRoleName, LoweredRoleName = newRoleName.ToLowerInvariant() });
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a role
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="throwOnPopulatedRole"></param>
        /// <returns></returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                var found = ctx.ExtendedRoles.FirstOrDefault(x => x.RoleName == roleName);
                EntityEntry<ExtendedRole> removed = null;
                if (found != null)
                {
                    removed = ctx.ExtendedRoles.Remove(found);
                    ctx.SaveChanges();
                }

                return removed != null;
            }
        }

        /// <summary>
        /// Gets all roles
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<IUIRole> GetAllRoles()
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                return ctx.ExtendedRoles.Select(x => new ExtendedUIRole { Name = x.RoleName, ProviderName = nameof(ExtendedRoleProvider) }).ToList();
            }
        }

        /// <summary>
        /// Gets all roles user can be assigned
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<string> GetAllRolesForUser(string userName)
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                return ctx.ExtendedRoles.Select(x => x.RoleName).ToList();
            }
        }

        /// <summary>
        /// Gets currently set roles for user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override IEnumerable<string> GetRolesForUser(string username)
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                var user = ctx.TblSynchedUser
                .Include(p => p.ExtendedUserRoles)
                .ThenInclude(p => p.ExtendedRole)
                .FirstOrDefault(u => u.UserName == username);

                if (user == null) { return Enumerable.Empty<string>(); }

                return user.ExtendedUserRoles.Select(x => x.ExtendedRole.RoleName).ToList();
            }
        }

        /// <summary>
        /// Gets the users in a role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override IEnumerable<string> GetUsersInRole(string roleName)
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                var roles = ctx.ExtendedRoles
                .Include(p => p.SynchedUserRoles)
                .ThenInclude(p => p.SynchedUser)
                .FirstOrDefault(r => r.LoweredRoleName == roleName.ToLowerInvariant());

                return roles?.SynchedUserRoles.Select(x => x.SynchedUser.UserName).ToList() ?? Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Determines if action is supported, always true for this provider
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public override bool IsSupported(ProviderActions action) => true;

        /// <summary>
        /// Removes a user from a role
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        public override void RemoveUserFromRole(string userName, string roleName) => RemoveUserFromRoles(userName, new string[] { roleName });

        /// <summary>
        /// Removes a user from given roles
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleNames"></param>
        public override void RemoveUserFromRoles(string username, IEnumerable<string> roleNames)
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                var user = ctx.TblSynchedUser
                .Include(p => p.ExtendedUserRoles)
                .ThenInclude(p => p.ExtendedRole)
                .FirstOrDefault(u => u.UserName == username);

                if (user == null)
                {
                    _logger.Information($"{username} was not found for role removal!");
                    return;
                }

                var rolesList = roleNames.ToList();
                var newRoles = user.ExtendedUserRoles.Where(x => !rolesList.Contains(x.ExtendedRole.RoleName)).ToList();

                user.ExtendedUserRoles = newRoles;

                ctx.TblSynchedUser.Update(user);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Removes given users from given roles
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void RemoveUsersFromRoles(IEnumerable<string> usernames, IEnumerable<string> roleNames)
        {
            foreach (var user in usernames)
            {
                RemoveUserFromRoles(user, roleNames);
            }
        }

        /// <summary>
        /// Checks if role exists
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool RoleExists(string roleName)
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                return ctx.ExtendedRoles.Any(r => roleName == r.RoleName);
            }
        }

        #endregion
    }
}