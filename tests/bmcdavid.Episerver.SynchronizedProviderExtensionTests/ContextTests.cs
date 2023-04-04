using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions.Tests
{
    [TestClass]
    public class ContextTests
    {
        [TestMethod]
        public void ShouldAddRole()
        {
            var role = CreateRole();

            using (var ctx = TestSetup.DbFactory.CreateContext())
            {
                ctx.ExtendedRoles.Add(role);
                ctx.SaveChanges();
            }

            using (var ctx = TestSetup.DbFactory.CreateContext())
            {
                Assert.IsTrue(ctx.ExtendedRoles.AsQueryable().Count() > 0);
                ctx.ExtendedRoles.Remove(role);
                ctx.SaveChanges();
            }
        }

        [TestMethod]
        public void ShouldAddUser()
        {
            var user = CreateUser(200);
            AddUser(user);

            void AssertCheck(ExtendedRoleDbContext ctx)
            {
                var userTest = ctx.TblSynchedUser
                    .AsNoTracking()
                    .Include(p => p.ExtendedUser).First();
                Assert.IsNotNull(userTest);
            }

            RemoveUser(user, AssertCheck);
        }

        [TestMethod]
        public async Task ShouldUpdateUsingExtendedUserRepositoryAsync()
        {
            var user = CreateUser(300);
            AddUser(user);
            var userTools = new ExtendedUserTools(TestSetup.DbFactory, null);
            var userProvider = new ExtendedUserProvider(TestSetup.DbFactory);
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            await userTools.SetExtendedRolesAsync(new ClaimsIdentity(claims, "UnitTest", ClaimTypes.Name, ClaimTypes.Role), new DateTime(2020, 01, 01));
            var testUser = userProvider.GetUser(user.UserName);

            Assert.IsFalse(testUser.CreationDate == DateTime.MinValue);
            Assert.IsTrue(testUser.CreationDate.Year == 2020);
            Assert.IsTrue(testUser.CreationDate == testUser.LastLoginDate);
            RemoveUser(user, ctx => { });
        }

        [TestMethod]
        public void ShouldAddUserToRole()
        {
            var role = CreateRole();
            var user = CreateUser();

            using (var ctx = TestSetup.DbFactory.CreateContext())
            {
                ctx.TblSynchedUser.Add(user);
                ctx.ExtendedRoles.Add(role);

                role.SynchedUserRoles.Add(new Entities.ExtendedRoleSynchedUser
                {
                    ExtendedRoleId = role.Id,
                    SynchedUserId = user.PkId
                });

                ctx.SaveChanges();
            }

            using (var ctx = TestSetup.DbFactory.CreateContext())
            {
                var sut = ctx.ExtendedRoles
                    .Include(i => i.SynchedUserRoles)
                    .ThenInclude(u => u.SynchedUser)
                    .FirstOrDefaultAsync()
                    .Result;

                Assert.IsTrue(sut != null && sut.SynchedUserRoles.Count > 0);
            }
        }

        [TestMethod]
        public void ShouldSearchUserProvider()
        {
            var i = 999;
            var searchUser = $"unitTest{i}";
            var user = CreateUser(i);
            AddUser(user);

            var sut = new ExtendedUserProvider(TestSetup.DbFactory);
            var emailSearch = sut.FindUsersByEmail($"nit{i}@", 1, 100, out var emailTotal);
            var userNameSearch = sut.FindUsersByName($"est{i}", 1, 100, out var userNameTotal);
            var usersearch = sut.GetUser($"unittest{i}");

            Assert.IsTrue(emailTotal == 1);
            Assert.IsTrue(emailSearch.First().Email == $"unit{i}@test.com");
            Assert.IsTrue(userNameTotal == 1);
            Assert.IsTrue(userNameSearch.First().Username == searchUser);
            Assert.IsTrue(usersearch?.Username == searchUser);
        }

        private static void AddUser(Entities.SynchedUser user)
        {
            using (var ctx = TestSetup.DbFactory.CreateContext())
            {
                ctx.TblSynchedUser.Add(user);
                ctx.SaveChanges();
            }
        }

        private static Entities.ExtendedRole CreateRole(int i = 0)
        {
            return new Entities.ExtendedRole
            {
                RoleName = $"UnitTesters{i}",
                LoweredRoleName = $"unittesters{i}"
            };
        }

        private static Entities.SynchedUser CreateUser(int i = 0)
        {
            return new Entities.SynchedUser
            {
                Email = $"unit{i}@test.com",
                UserName = $"unitTest{i}",
                LoweredUserName = $"unittest{i}",
                GivenName = "Unit",
                LoweredGivenName = "unit",
                Surname = "Test",
                LoweredSurname = "test"
            };
        }

        private static void RemoveUser(Entities.SynchedUser user, Action<ExtendedRoleDbContext> action)
        {
            using (var ctx = TestSetup.DbFactory.CreateContext())
            {
                action(ctx);
                ctx.TblSynchedUser.Remove(user);
                ctx.SaveChanges();
            }
        }
    }
}
