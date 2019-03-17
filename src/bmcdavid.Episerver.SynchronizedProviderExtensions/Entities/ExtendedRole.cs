using System.Collections.Generic;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions.Entities
{
    /// <summary>
    /// Extended Role Entity
    /// </summary>
    public class ExtendedRole
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Role Name
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Lowered Role Name
        /// </summary>
        public string LoweredRoleName { get; set; }

        /// <summary>
        /// Mappings to synchronized users
        /// </summary>
        public ICollection<ExtendedRoleSynchedUser> SynchedUserRoles { get; set; } = new List<ExtendedRoleSynchedUser>();
    }
}
