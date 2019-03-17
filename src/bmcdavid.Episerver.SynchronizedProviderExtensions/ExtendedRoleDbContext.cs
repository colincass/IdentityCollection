using bmcdavid.Episerver.SynchronizedProviderExtensions.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions
{
    /// <summary>
    /// EF Core DbContext for user/role extensions
    /// </summary>
    public sealed class ExtendedRoleDbContext : DbContext
    {
        /// <summary>
        /// Constructor for project migrations
        /// </summary>
        [Obsolete("Use the constructor with arguments")]
        public ExtendedRoleDbContext() { }

        /// <summary>
        /// Constructor to use
        /// </summary>
        /// <param name="options"></param>
        public ExtendedRoleDbContext(IDbContextSettings options) : base(options.ContextOptions) { }

        /// <summary>
        /// Synched user entity table
        /// </summary>
        public DbSet<SynchedUser> TblSynchedUser { get; set; }

        /// <summary>
        /// Manually managed UI roles
        /// </summary>
        public DbSet<ExtendedRole> ExtendedRoles { get; set; }

        /// <summary>
        /// Default configuring uses localhost server with table ExtendedEpiSecurity
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) { return; }

            optionsBuilder.UseSqlServer("Server=localhost;Database=ExtendedEpiSecurity;Trusted_Connection=True;");
        }

        /// <summary>
        /// Entity mappings
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // for reference only, table created by Episerver
            modelBuilder.Entity<SynchedUser>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_tblWindowsUser");
                entity.ToTable("tblSynchedUser");
                entity.HasIndex(e => e.Email)
                    .HasName("IX_tblWindowsUser_Email");
                entity.HasIndex(e => e.LoweredGivenName)
                    .HasName("IX_tblWindowsUser_LoweredGivenName");
                entity.HasIndex(e => e.LoweredSurname)
                    .HasName("IX_tblWindowsUser_LoweredSurname");
                entity.HasIndex(e => e.LoweredUserName)
                    .HasName("IX_tblWindowsUser_Unique")
                    .IsUnique();
                entity.Property(e => e.PkId).HasColumnName("pkID");
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.GivenName).HasMaxLength(255);
                entity.Property(e => e.LoweredGivenName).HasMaxLength(255);
                entity.Property(e => e.LoweredSurname).HasMaxLength(255);
                entity.Property(e => e.LoweredUserName)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.Surname).HasMaxLength(255);
                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ExtendedRole>(entity =>
            {
                entity.ToTable("tblSynchedRolesExtensions");

                entity.HasIndex(e => e.LoweredRoleName)
                    .HasName("IX_tblSynchedRolesExtensions_LoweredRoleName");

                entity
                .Property(p => p.RoleName)
                .IsRequired()
                .HasMaxLength(255);

                entity
                .Property(p => p.LoweredRoleName)
                .IsRequired()
                .HasMaxLength(255);
            });

            modelBuilder.Entity<ExtendedRoleSynchedUser>(entity =>
            {
                entity.ToTable("tblSynchedUserToRolesExtensions")
                    .HasKey(bc => new { bc.SynchedUserId, bc.ExtendedRoleId });

                entity.HasOne(b => b.SynchedUser)
                    .WithMany(b => b.ExtendedUserRoles)
                    .HasForeignKey(b => b.SynchedUserId);

                entity.HasOne(b => b.ExtendedRole)
                    .WithMany(b => b.SynchedUserRoles)
                    .HasForeignKey(bc => bc.ExtendedRoleId);
            });

            modelBuilder.Entity<ExtendedSynchedUser>(entity =>
            {
                entity.ToTable("tblSynchedUserExtensions")
                    .HasKey(k => new { k.Id });
            });

            modelBuilder.Entity<SynchedUser>()
                .HasOne(a => a.ExtendedUser)
                .WithOne(b => b.SynchedUser)
                .HasForeignKey<ExtendedSynchedUser>(b => b.SynchedUserId);
        }
    }
}