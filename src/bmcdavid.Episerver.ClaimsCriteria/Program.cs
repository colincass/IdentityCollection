using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bmcdavid.Episerver.ClaimsCriteria
{
    /// <summary>
    /// Configure Dependency Injection
    /// </summary>
    public static class ClaimsCriteriaExtension
    {
        /// <summary>
        /// DI Configuration
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddClaimsCriteriaServices(this IServiceCollection services)
        {
            return services.AddSingleton<IClaimTypeSelectorExtension, DefaultClaimTypeSelectorExtension>()
                .AddSingleton<IClaimUserTools, DefaultClaimUserTools>();
        }
    }
}
