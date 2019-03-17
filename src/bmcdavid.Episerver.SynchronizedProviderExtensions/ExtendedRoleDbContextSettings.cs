using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// Settings builder for ExtendedRoleDbContext
    /// </summary>
    public class ExtendedRoleDbContextSettings : IDbContextSettings
    {
        private DbContextOptions _options;

        /// <summary>EpiserverDB connection options</summary>
        public DbContextOptions ContextOptions => BuildOptions();

        /// <summary>Run migrations for episerver, default is true</summary>
        public bool RunMigrations { get; } = true;

        private DbContextOptions BuildOptions()
        {
            if (_options != null)
                return _options;

            DbContextOptionsBuilder<ExtendedRoleDbContext> optionsBuilder = new DbContextOptionsBuilder<ExtendedRoleDbContext>();
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["EPiServerDB"].ConnectionString);
            _options = optionsBuilder.Options;
            return _options;
        }
    }
}