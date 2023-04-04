using bmcdavid.Episerver.SynchronizedProviderExtensions.Entities;
using EPiServer.Notification;
using EPiServer.Security;
using EPiServer.Shell.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// Extended User provider for Episerver UI
    /// </summary>
    public class ExtendedUserProvider : UIUserProvider, IQueryableNotificationUsers
    {
        private readonly ExtendedRoleDbContextFactory _episerverDbContextFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="episerverDbContextFactory"></param>
        public ExtendedUserProvider(ExtendedRoleDbContextFactory episerverDbContextFactory)
        {
            _episerverDbContextFactory = episerverDbContextFactory ?? throw new ArgumentNullException(nameof(episerverDbContextFactory));
        }

        /// <summary>
        /// Enabled, always true
        /// </summary>
        public override bool Enabled => true;

        /// <summary>
        /// Provider name
        /// </summary>
        public override string Name => nameof(ExtendedUserProvider);

        /// <summary>
        /// Asynchronous Count
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<int> CountAsync(Expression<Func<IUIUser, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT SUPPORTED, will throw exception
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="status"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public override IUIUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, out UIUserCreateStatus status, out IEnumerable<string> errors) => throw new NotImplementedException();

        /// <summary>
        /// NOT SUPPORTED, always false
        /// </summary>
        /// <param name="username"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData) => false;

        /// <summary>
        /// Finds part of user 
        /// </summary>
        /// <param name="partOfUser"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedNotificationUserResult> FindAsync(string partOfUser, int pageIndex, int pageSize)
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                pageIndex = pageIndex < 1 ? 1 : pageIndex;
                var count = ctx.TblSynchedUser.Count(u => u.UserName.Contains(partOfUser));
                var results = await ctx.TblSynchedUser.AsAsyncEnumerable().Where(u => u.UserName.Contains(partOfUser))
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagedNotificationUserResult(results.Select(u => new NotificationUser(u.UserName)), count);
            }
        }

        /// <summary>
        /// Find  by email
        /// </summary>
        /// <param name="emailToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override IEnumerable<IUIUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords) =>
            Search(pageIndex, pageSize, out totalRecords, emailToMatch, emailSearch: true);

        /// <summary>
        /// Find by username
        /// </summary>
        /// <param name="usernameToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override IEnumerable<IUIUser> FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords) =>
            Search(pageIndex, pageSize, out totalRecords, usernameToMatch);

        /// <summary>
        /// Gets paged users
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override IEnumerable<IUIUser> GetAllUsers(int pageIndex, int pageSize, out int totalRecords) =>
            Search(pageIndex, pageSize, out totalRecords);

        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override IUIUser GetUser(string username)
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                return ConvertToUIUser(ctx.TblSynchedUser.Include(f => f.ExtendedUser).FirstOrDefault(u => u.UserName == username));
            }
        }

        /// <summary>
        /// Determines action supported
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public override bool IsSupported(string providerName, string propertyName)
        {
            return propertyName == "email" ? true : base.IsSupported(providerName, propertyName);
        }

        /// <summary>
        /// Determines action supported
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public override bool IsSupported(string providerName, ProviderActions action)
        {
            if ((action == ProviderActions.Delete || action == ProviderActions.Update) && providerName == nameof(ExtendedRoleProvider))
            {
                return true;
            }

            return action != ProviderActions.Create;
        }

        /// <summary>
        /// Updates a user, really does nothing but needs to be allowed to support adding roles
        /// </summary>
        /// <param name="user"></param>
        public override void UpdateUser(IUIUser user) { }

        /// <summary>
        /// Converts entity to UI user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual UIUser ConvertToUIUser(SynchedUser user)
        {
            if (user == null) return null;

            return new UIUser
            {
                Username = user.UserName,
                IsApproved = true,
                ProviderName = Name,
                Email = user.Email,
                Comment = ((user.GivenName ?? string.Empty) + " " + (user.Surname ?? string.Empty)).Trim(),
                LastLoginDate = user.ExtendedUser?.LastLoginUtcDate.ToLocalTime(),
                CreationDate = user.ExtendedUser?.CreatedUtcDate.ToLocalTime() ?? DateTime.MinValue
            };
        }

        private IEnumerable<IUIUser> Search(int pageIndex, int pageSize, out int totalRecords, string toMatch = "", bool emailSearch = false)
        {
            using (var ctx = _episerverDbContextFactory.CreateContext())
            {
                pageIndex = pageIndex < 1 ? 1 : pageIndex;
                totalRecords = ctx.TblSynchedUser
                    .Count(u => emailSearch ? u.Email.Contains(toMatch) : u.UserName.Contains(toMatch));

                if (totalRecords == 0) { return Enumerable.Empty<UIUser>(); }

                var matches = ctx.TblSynchedUser.AsQueryable()
                    .Where(u => emailSearch ? u.Email.Contains(toMatch) : u.UserName.Contains(toMatch))
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);

                return matches.Select(ConvertToUIUser);
            }
        }
    }
}