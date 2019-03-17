using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Notification;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Security;
using Microsoft.EntityFrameworkCore;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// Init module to assign services for extended roles
    /// </summary>
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ExtendedRoleConfigurationModule : IConfigurableModule
    {
        /// <summary>
        /// Adds services
        /// </summary>
        /// <param name="context"></param>
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.RemoveAll<UIUserProvider>();
            context.Services.RemoveAll<UIRoleProvider>();

            context.Services
                .AddSingleton<IDbContextSettings, ExtendedRoleDbContextSettings>()
                .AddSingleton<ExtendedRoleDbContextFactory>()
                .AddSingleton<IExtendedUserTools, ExtendedUserTools>()
                .AddSingleton<IQueryableNotificationUsers, ExtendedUserProvider>()
                .AddSingleton<UIUserProvider, ExtendedUserProvider>()
                .AddSingleton<UIRoleProvider, ExtendedRoleProvider>()
                .AddTransient<SecurityEntityProvider, ExtendedSecurityProvider>()
            ;
        }

        /// <summary>
        /// Migrates db if enabled
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(InitializationEngine context)
        {
            if (context.Locate.Advanced.GetInstance<IDbContextSettings>().RunMigrations)
            {
                using (var db = context.Locate.Advanced.GetInstance<ExtendedRoleDbContextFactory>().CreateContext())
                {
                    db.Database.Migrate();
                }
            }
        }

        /// <summary>
        /// Uninit
        /// </summary>
        /// <param name="context"></param>
        public void Uninitialize(InitializationEngine context) { }
    }
}