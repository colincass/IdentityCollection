using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bmcdavid.Episerver.SynchronizedProviderExtensions.Migrations
{
    /// <summary>
    /// Db migration
    /// </summary>
    public partial class SetupExtendedRoles : Migration
    {
        /// <summary>
        /// Up
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblSynchedRolesExtensions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(maxLength: 255, nullable: false),
                    LoweredRoleName = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSynchedRolesExtensions", x => x.Id);
                });

            migrationBuilder.Sql(@"
/****** Object:  Table [dbo].[tblSynchedUser]   ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID(N'dbo.tblSynchedUser', N'U') IS NULL 
BEGIN
	CREATE TABLE [dbo].[tblSynchedUser](
		[pkID] [int] IDENTITY(1,1) NOT NULL,
		[UserName] [nvarchar](255) NOT NULL,
		[LoweredUserName] [nvarchar](255) NOT NULL,
		[Email] [nvarchar](255) NULL,
		[GivenName] [nvarchar](255) NULL,
		[LoweredGivenName] [nvarchar](255) NULL,
		[Surname] [nvarchar](255) NULL,
		[LoweredSurname] [nvarchar](255) NULL,
		[Metadata] [nvarchar](max) NULL,
		CONSTRAINT [PK_tblWindowsUser] PRIMARY KEY CLUSTERED 
	(
		[pkID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
            ");

            //migrationBuilder.CreateTable(
            //    name: "tblSynchedUser",
            //    columns: table => new
            //    {
            //        pkID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        UserName = table.Column<string>(maxLength: 255, nullable: false),
            //        LoweredUserName = table.Column<string>(maxLength: 255, nullable: false),
            //        Email = table.Column<string>(maxLength: 255, nullable: true),
            //        GivenName = table.Column<string>(maxLength: 255, nullable: true),
            //        LoweredGivenName = table.Column<string>(maxLength: 255, nullable: true),
            //        Surname = table.Column<string>(maxLength: 255, nullable: true),
            //        LoweredSurname = table.Column<string>(maxLength: 255, nullable: true),
            //        Metadata = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tblWindowsUser", x => x.pkID);
            //    });

            migrationBuilder.CreateTable(
                name: "tblSynchedUserExtensions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SynchedUserId = table.Column<int>(nullable: false),
                    CreatedUtcDate = table.Column<DateTime>(nullable: false),
                    LastLoginUtcDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSynchedUserExtensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblSynchedUserExtensions_tblSynchedUser_SynchedUserId",
                        column: x => x.SynchedUserId,
                        principalTable: "tblSynchedUser",
                        principalColumn: "pkID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblSynchedUserToRolesExtensions",
                columns: table => new
                {
                    SynchedUserId = table.Column<int>(nullable: false),
                    ExtendedRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSynchedUserToRolesExtensions", x => new { x.SynchedUserId, x.ExtendedRoleId });
                    table.ForeignKey(
                        name: "FK_tblSynchedUserToRolesExtensions_tblSynchedRolesExtensions_ExtendedRoleId",
                        column: x => x.ExtendedRoleId,
                        principalTable: "tblSynchedRolesExtensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblSynchedUserToRolesExtensions_tblSynchedUser_SynchedUserId",
                        column: x => x.SynchedUserId,
                        principalTable: "tblSynchedUser",
                        principalColumn: "pkID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblSynchedRolesExtensions_LoweredRoleName",
                table: "tblSynchedRolesExtensions",
                column: "LoweredRoleName");

            //migrationBuilder.CreateIndex(
            //    name: "IX_tblWindowsUser_Email",
            //    table: "tblSynchedUser",
            //    column: "Email");

            //migrationBuilder.CreateIndex(
            //    name: "IX_tblWindowsUser_LoweredGivenName",
            //    table: "tblSynchedUser",
            //    column: "LoweredGivenName");

            //migrationBuilder.CreateIndex(
            //    name: "IX_tblWindowsUser_LoweredSurname",
            //    table: "tblSynchedUser",
            //    column: "LoweredSurname");

            //migrationBuilder.CreateIndex(
            //    name: "IX_tblWindowsUser_Unique",
            //    table: "tblSynchedUser",
            //    column: "LoweredUserName",
            //    unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblSynchedUserExtensions_SynchedUserId",
                table: "tblSynchedUserExtensions",
                column: "SynchedUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblSynchedUserToRolesExtensions_ExtendedRoleId",
                table: "tblSynchedUserToRolesExtensions",
                column: "ExtendedRoleId");
        }

        /// <summary>
        /// Down
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblSynchedUserExtensions");

            migrationBuilder.DropTable(
                name: "tblSynchedUserToRolesExtensions");

            migrationBuilder.DropTable(
                name: "tblSynchedRolesExtensions");

            //migrationBuilder.DropTable(
            //    name: "tblSynchedUser");
        }
    }
}
