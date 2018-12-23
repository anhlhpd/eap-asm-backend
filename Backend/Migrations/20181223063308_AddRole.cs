using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class AddRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: false),
                    RoleStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: "ADMIN",
                columns: new[] { "CreatedAt", "DeletedAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2018, 12, 23, 13, 33, 8, 18, DateTimeKind.Local), new DateTime(2018, 12, 23, 13, 33, 8, 19, DateTimeKind.Local), "lD2Hql+8vRsCYWlUL0R0n0PBvfyuXWNthViiPJmg7kE=", "ft3f24DkXBw1aB3BqyZLGw==", new DateTime(2018, 12, 23, 13, 33, 8, 19, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: "ADMIN",
                columns: new[] { "CreatedAt", "DeletedAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2018, 12, 23, 13, 29, 19, 675, DateTimeKind.Local), new DateTime(2018, 12, 23, 13, 29, 19, 676, DateTimeKind.Local), "g03uMh/PP5hVk4wjG9XLaKEpxvNl9NbAoTsbXSFNiJQ=", "kcVVlaX/f6QMs23idoUzMg==", new DateTime(2018, 12, 23, 13, 29, 19, 676, DateTimeKind.Local) });
        }
    }
}
