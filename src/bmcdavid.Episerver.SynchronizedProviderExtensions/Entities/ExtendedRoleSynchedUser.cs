namespace bmcdavid.Episerver.SynchronizedProviderExtensions.Entities
{
    /// <summary>
    /// Mapping entity for an extended role to synched user
    /// </summary>
    public class ExtendedRoleSynchedUser
    {
        /// <summary>
        /// Synched User Id
        /// </summary>
        public int SynchedUserId { get; set; }

        /// <summary>
        /// Synched User object, may be null if not included by context
        /// </summary>
        public SynchedUser SynchedUser { get; set; }

        /// <summary>
        /// Extended role Id
        /// </summary>
        public int ExtendedRoleId { get; set; }

        /// <summary>
        /// Extended role object, may be null if not included by context
        /// </summary>
        public ExtendedRole ExtendedRole { get; set; }
    }
}
