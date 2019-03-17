using Microsoft.EntityFrameworkCore;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions.Tests
{
    public class UnitTestDbSettings : IDbContextSettings
    {
        private readonly string _dbName;
        private DbContextOptions _options;

        public UnitTestDbSettings(string dbName) => _dbName = dbName;

        public DbContextOptions ContextOptions => BuildOptions();

        public bool RunMigrations { get; } = true;

        private DbContextOptions BuildOptions()
        {
            if (_options != null)
            {
                return _options;
            }

            DbContextOptionsBuilder<ExtendedRoleDbContext> optionsBuilder = new DbContextOptionsBuilder<ExtendedRoleDbContext>();
            optionsBuilder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database={_dbName};Trusted_Connection=True;");
            return _options = optionsBuilder.Options;
        }
    }
}
