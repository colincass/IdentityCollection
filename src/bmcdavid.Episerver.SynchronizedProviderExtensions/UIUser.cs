#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using EPiServer.Shell.Security;
using System;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    public class UIUser : IUIUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public string PasswordQuestion { get; }
        public string ProviderName { get; set; }
        public string Comment { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastLockoutDate { get; set; }
    }
}