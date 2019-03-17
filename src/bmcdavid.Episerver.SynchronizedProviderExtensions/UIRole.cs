#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using EPiServer.Shell.Security;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    public class ExtendedUIRole : IUIRole
    {
        public string Name { get; set; }
        public string ProviderName { get; set; }
    }
}