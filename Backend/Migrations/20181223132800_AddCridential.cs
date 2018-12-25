using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class AddCridential : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Credential",
                columns: table => new
                {
                    AccountId = table.Column<string>(nullable: false),
                    AccessToken = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    AccountId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credential", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Credential_Account_AccountId1",
                        column: x => x.AccountId1,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: "ADMIN",
                columns: new[] { "CreatedAt", "DeletedAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2018, 12, 23, 20, 27, 59, 539, DateTimeKind.Local), new DateTime(2018, 12, 23, 20, 27, 59, 542, DateTimeKind.Local), "hpUtYPqu784HFGnHlg2rbEzoYdExrwVkXR15ImEsEo8=", "uq5jU9bWymm3UgQlU8ef0Q==", new DateTime(2018, 12, 23, 20, 27, 59, 542, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_Credential_AccountId1",
                table: "Credential",
                column: "AccountId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credential");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: "ADMIN",
                columns: new[] { "CreatedAt", "DeletedAt", "Password", "Salt", "UpdatedAt" },
                values: new object[] { new DateTime(2018, 12, 23, 13, 33, 8, 18, DateTimeKind.Local), new DateTime(2018, 12, 23, 13, 33, 8, 19, DateTimeKind.Local), "lD2Hql+8vRsCYWlUL0R0n0PBvfyuXWNthViiPJmg7kE=", "ft3f24DkXBw1aB3BqyZLGw==", new DateTime(2018, 12, 23, 13, 33, 8, 19, DateTimeKind.Local) });
        }
    }
}
