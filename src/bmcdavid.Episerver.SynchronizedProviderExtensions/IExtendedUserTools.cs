using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;


namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// Tools to extend claim identity
    /// </summary>
    public interface IExtendedUserTools
    {
        /// <summary>
        /// Adds role claims for any matching visitor group
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        Task AddVisitorGroupRolesAsClaimsAsync(ClaimsIdentity identity, HttpContext httpContext = null);

        /// <summary>
        /// Sets login and created utc dates and assigns manual roles to identity
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="loginDateUtc"></param>
        /// <returns></returns>
        Task SetExtendedRolesAsync(ClaimsIdentity identity, DateTime? loginDateUtc = null);
    }
}