using EPiServer.Security;
using System.Collections.Generic;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// Compares by name
    /// </summary>
    public class SecurityNameComparer : IEqualityComparer<SecurityEntity>
    {
        /// <summary>
        /// Compares name
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(SecurityEntity x, SecurityEntity y) => x?.Name == y?.Name;

        /// <summary>
        /// Name hash code
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(SecurityEntity obj) => obj.Name.GetHashCode();
    }
}
