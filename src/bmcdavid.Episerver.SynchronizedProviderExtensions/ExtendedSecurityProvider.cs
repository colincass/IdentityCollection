﻿using EPiServer.Security;
using EPiServer.Shell.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// Security provider which combines synched user roles with manual extended roles
    /// </summary>
    public class ExtendedSecurityProvider : SecurityEntityProvider
    {
        private readonly SecurityEntityProvider _synchedProvider;
        private readonly UIRoleProvider _extendedRoleProvider;
        private readonly UIUserProvider _extendedUserProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="synchedProvider"></param>
        /// <param name="extendedRoleProvider"></param>
        /// <param name="uIUserProvider"></param>
        public ExtendedSecurityProvider(SynchronizingRolesSecurityEntityProvider synchedProvider, UIRoleProvider extendedRoleProvider, UIUserProvider uIUserProvider)
        {
            _synchedProvider = synchedProvider;
            _extendedRoleProvider = extendedRoleProvider;
            _extendedUserProvider = uIUserProvider;
        }

        /// <summary>
        /// Gets manual and synched roles for user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Obsolete]
        public override IEnumerable<string> GetRolesForUser(string userName)
        {
            var defaultRoles = new HashSet<string>(_synchedProvider.GetRolesForUser(userName));
            var extendedRoles = _extendedRoleProvider.GetRolesForUser(userName);

            foreach (var role in extendedRoles)
            {
                defaultRoles.Add(role);
            }

            return defaultRoles;
        }

        /// <summary>
        /// Gets manual and synched roles for user asynchronously
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async override Task<IEnumerable<string>> GetRolesForUserAsync(string userName)
        {
            var defaultRoles = new HashSet<string>(await _synchedProvider.GetRolesForUserAsync(userName));
            var extendedRoles = _extendedRoleProvider.GetRolesForUserAsync(userName);

            await foreach (var role in extendedRoles)
            {
                defaultRoles.Add(role);
            }

            return defaultRoles;
        }

        /// <summary>
        /// Called by UI to assign permissions
        /// </summary>
        /// <param name="partOfValue"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        [Obsolete]
        public override IEnumerable<SecurityEntity> Search(string partOfValue, string claimType)
        {
            switch (claimType)
            {
                case ClaimTypes.Email:
                    return _extendedUserProvider.FindUsersByEmail(partOfValue, 1, 100, out var total)
                        .Select(x => new SecurityEntity(x.Username, SecurityEntityType.User));

                case ClaimTypes.Name:
                    return _synchedProvider.Search(partOfValue, claimType);

                case ClaimTypes.Role:
                default:
                    var hashSet = new HashSet<SecurityEntity>(_synchedProvider.Search(partOfValue, claimType), new SecurityNameComparer());
                    var extendedRoles = _extendedRoleProvider.GetAllRoles();

                    if (!string.IsNullOrWhiteSpace(partOfValue))
                    {
                        extendedRoles = extendedRoles.Where(r => r.Name.Contains(partOfValue));
                    }

                    foreach (var manual in extendedRoles)
                    {
                        hashSet.Add(new SecurityEntity(manual.Name, SecurityEntityType.Role));
                    }

                    return hashSet.OrderBy(x => x.Name).ToList();
            }
        }

        /// <summary>
        /// Searches user
        /// </summary>
        /// <param name="partOfValue"></param>
        /// <param name="claimType"></param>
        /// <param name="startIndex"></param>
        /// <param name="maxRows"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public override IEnumerable<SecurityEntity> Search(string partOfValue, string claimType, int startIndex, int maxRows, out int totalCount)
        {
            var defaultResults = _synchedProvider.Search(partOfValue, claimType, startIndex, maxRows, out totalCount);

            switch (claimType)
            {
                case ClaimTypes.Role:
                    var hashSet = new HashSet<SecurityEntity>(defaultResults, new SecurityNameComparer());
                    var filteredRoles = _extendedRoleProvider.GetAllRoles().Where(r => r.Name.Contains(partOfValue));

                    foreach (var manual in filteredRoles)
                    {
                        hashSet.Add(new SecurityEntity(manual.Name, SecurityEntityType.Role));
                        totalCount++;
                    }

                    return hashSet.OrderBy(x => x.Name).ToList();

                case ClaimTypes.Email:
                case ClaimTypes.Name:
                default:
                    return defaultResults;
            }

        }
    }
}