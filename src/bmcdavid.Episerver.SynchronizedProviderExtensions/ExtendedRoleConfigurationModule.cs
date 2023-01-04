using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Notification;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// Init module to assign services for extended roles
    /// </summary>
    public static class ExtendedRoleConfigurationModule
    {
        /// <summary>
        /// Adds services
        /// </summary>
        /// <param name="context"></param>
        public static IServiceCollection AddSynchronizedProviderExtensions(this IServiceCollection services)
        {
            var userProviderServiceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(UIUserProvider));
            var roleProviderServiceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(UIRoleProvider));

            if (userProviderServiceDescriptor != null)
            {
                services.Remove(userProviderServiceDescriptor);
            }

            if (roleProviderServiceDescriptor != null)
            {
                services.Remove(roleProviderServiceDescriptor);
            }

            return services
                .AddSingleton<IDbContextSettings, ExtendedRoleDbContextSettings>()
                .AddSingleton<ExtendedRoleDbContextFactory>()
                .AddSingleton<IExtendedUserTools, ExtendedUserTools>()
                .AddSingleton<IQueryableNotificationUsers, ExtendedUserProvider>()
                .AddSingleton<UIUserProvider, ExtendedUserProvider>()
                .AddSingleton<UIRoleProvider, ExtendedRoleProvider>()
                .AddTransient<SecurityEntityProvider, ExtendedSecurityProvider>();
        }
    }
}