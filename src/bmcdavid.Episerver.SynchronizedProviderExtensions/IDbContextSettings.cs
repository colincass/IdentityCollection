using Microsoft.EntityFrameworkCore;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// DbContext options
    /// </summary>
    public interface IDbContextSettings
    {
        /// <summary>Connection options</summary>
        DbContextOptions ContextOptions { get; }

        /// <summary>
        /// Determines if migrations run are init.
        /// </summary>
        bool RunMigrations { get; }
    }
}