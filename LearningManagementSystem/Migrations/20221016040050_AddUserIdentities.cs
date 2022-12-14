using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
	public partial class AddUserIdentities : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "UserIdentities",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
					PasswordResetDate = table.Column<DateTime>(type: "datetime2", nullable: true),
					LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserIdentities", x => x.UserId);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "UserIdentities");
		}
	}
}
