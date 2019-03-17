using System;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions.Entities
{
    /// <summary>
    /// Extended attributes for a synched user object
    /// </summary>
    public class ExtendedSynchedUser
    {
        /// <summary>
        /// Extended attributes Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Synched User
        /// </summary>
        public SynchedUser SynchedUser { get; set; }

        /// <summary>
        /// Synched User Id
        /// </summary>
        public int SynchedUserId { get; set; }

        /// <summary>
        /// Date synched user was created
        /// </summary>
        public DateTime CreatedUtcDate { get; set; }

        /// <summary>
        /// Last login to Episerver for synched user
        /// </summary>
        public DateTime LastLoginUtcDate { get; set; }
    }
}
