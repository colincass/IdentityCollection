namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// Creates ExtendedRoleDbContext instances, considered safe to inject into singleton classes
    /// </summary>
    public class ExtendedRoleDbContextFactory
    {
        private readonly IDbContextSettings _dbContextSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContextSettings"></param>
        public ExtendedRoleDbContextFactory(IDbContextSettings dbContextSettings) => _dbContextSettings = dbContextSettings;

        /// <summary>
        /// Creates an ExtendedRoleDbContext
        /// </summary>
        /// <returns></returns>
        public virtual ExtendedRoleDbContext CreateContext() => new ExtendedRoleDbContext(options: _dbContextSettings);
    }
}