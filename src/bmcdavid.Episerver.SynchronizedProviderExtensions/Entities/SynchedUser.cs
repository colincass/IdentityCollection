using System.Collections.Generic;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions.Entities
{
    /// <summary>
    /// Synched user created by Episerver schema
    /// </summary>
    public class SynchedUser
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int PkId { get; set; }
        
        /// <summary>
        /// Username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Lowered username
        /// </summary>
        public string LoweredUserName { get; set; }

        /// <summary>
        /// Email, may be null if claim not set
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Given/First name, may be null
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Lowered given/first name, may be null
        /// </summary>
        public string LoweredGivenName { get; set; }

        /// <summary>
        /// Sur/last name, may be null
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Lowered sur/last name, may be null
        /// </summary>
        public string LoweredSurname { get; set; }

        /// <summary>
        /// Extra claims included in identity when logged in
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// Mapping for user extension
        /// </summary>
        public ExtendedSynchedUser ExtendedUser { get; set; }

        /// <summary>
        /// Mapping for extended roles
        /// </summary>
        public ICollection<ExtendedRoleSynchedUser> ExtendedUserRoles { get; set; } = new List<ExtendedRoleSynchedUser>();
    }
}