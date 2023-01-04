using EPiServer.Cms.Shell;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions.Migrations
{
    internal class ExtendedRoleDbContextInitializationModule : IInitializableModule
    {
        /// <summary>
        /// Migrates db if enabled
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(InitializationEngine context)
        {
            if (context.Locate.Advanced.GetService<IDbContextSettings>().RunMigrations)
            {
                using (var db = context.Locate.Advanced.GetService<ExtendedRoleDbContextFactory>().CreateContext())
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

