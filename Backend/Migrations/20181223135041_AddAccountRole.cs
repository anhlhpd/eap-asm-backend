using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class AddAccountRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    AccountId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRoles", x => new { x.AccountId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AccountRoles_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: "ADMIN",
                columns: new[] { "CreatedAt", "DeletedAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2018, 12, 23, 20, 50, 40, 564, DateTimeKind.Local), new DateTime(2018, 12, 23, 20, 50, 40, 565, DateTimeKind.Local), "xFqwpzKM8kAhDCYDIWu1rg8bikCAMa1k7elxc1H82K4=", "6lYr+zM81vlg9t/ngiIr/g==", new DateTime(2018, 12, 23, 20, 50, 40, 565, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRoles_RoleId",
                table: "AccountRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRoles");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: "ADMIN",
                columns: new[] { "CreatedAt", "DeletedAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2018, 12, 23, 20, 27, 59, 539, DateTimeKind.Local), new DateTime(2018, 12, 23, 20, 27, 59, 542, DateTimeKind.Local), "hpUtYPqu784HFGnHlg2rbEzoYdExrwVkXR15ImEsEo8=", "uq5jU9bWymm3UgQlU8ef0Q==", new DateTime(2018, 12, 23, 20, 27, 59, 542, DateTimeKind.Local) });
        }
    }
}
