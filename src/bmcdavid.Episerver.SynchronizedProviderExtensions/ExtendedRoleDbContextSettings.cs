using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// Settings builder for ExtendedRoleDbContext
    /// </summary>
    public class ExtendedRoleDbContextSettings : IDbContextSettings
    {
        private DbContextOptions _options;
        private IConfiguration _configuration;

        /// <summary>
        /// Constructor with injected site configuration
        /// </summary>
        /// <param name="configuration"></param>
        public ExtendedRoleDbContextSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>EpiserverDB connection options</summary>
        public DbContextOptions ContextOptions => BuildOptions();

        /// <summary>Run migrations for episerver, default is true</summary>
        public bool RunMigrations { get; } = true;

        private DbContextOptions BuildOptions()
        {
            if (_options != null)
                return _options;

            DbContextOptionsBuilder<ExtendedRoleDbContext> optionsBuilder = new DbContextOptionsBuilder<ExtendedRoleDbContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("EPiServerDB"));
            _options = optionsBuilder.Options;
            return _options;
        }
    }
}