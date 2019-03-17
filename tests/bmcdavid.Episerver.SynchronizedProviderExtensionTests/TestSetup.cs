using DotNetStarter.IntegrationTestTools;
using DotNetStarter.IntegrationTestTools.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions.Tests
{
    [TestClass]
    public class TestSetup
    {
        static IDbIntegrationTestSetup DbIntegrationTestSetup;

        internal static ExtendedRoleDbContextFactory DbFactory { get; private set; }

        private const string DbName = "ExtendedEpiSecurityUnitTest";

        [AssemblyCleanup]
        public static void Cleanup()
        {
            DbIntegrationTestSetup.Shutdown();
        }

        [AssemblyInitialize]
        public static void Init(TestContext testContext)
        {
            DbFactory = new ExtendedRoleDbContextFactory(new UnitTestDbSettings(DbName));

            DbIntegrationTestSetup = new LocalDbIntegrationTestSetup()
                .SetDatabaseName(DbName)
                .Startup(() =>
                {
                    using (var ctx = DbFactory.CreateContext())
                    {
                        ctx.Database.Migrate();
                    }
                });
        }
    }
}
