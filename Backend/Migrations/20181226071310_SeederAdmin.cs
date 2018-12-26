using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class SeederAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "AccountId", "AccountStatus", "CreatedAt", "DeletedAt", "Email", "Password", "Salt", "UpdatedAt", "Username" },
                values: new object[] { "ADMIN", 1, new DateTime(2018, 12, 26, 14, 13, 9, 824, DateTimeKind.Local), new DateTime(2018, 12, 26, 14, 13, 9, 825, DateTimeKind.Local), "admin@admin.com", "99HskOE6DozfKApuZx05xssL3teWKh3F/gI6LDdO+I0=", "nG9F6wUwm2unLajdOp2lOw==", new DateTime(2018, 12, 26, 14, 13, 9, 825, DateTimeKind.Local), "ADMIN" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "CreatedAt", "DeletedAt", "Description", "Name", "RoleStatus", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2018, 12, 26, 14, 13, 9, 840, DateTimeKind.Local), new DateTime(2018, 12, 26, 14, 13, 9, 840, DateTimeKind.Local), null, "Admin", 1, new DateTime(2018, 12, 26, 14, 13, 9, 840, DateTimeKind.Local) });

            migrationBuilder.InsertData(
                table: "AccountRoles",
                columns: new[] { "RoleId", "AccountId" },
                values: new object[] { 1, "Admin" });

            migrationBuilder.InsertData(
                table: "PersonalInformation",
                columns: new[] { "AccountId", "Birthday", "FirstName", "Gender", "LastName", "Phone" },
                values: new object[] { "ADMIN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN", 2, "ADMIN", "01234567890" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountRoles",
                keyColumns: new[] { "RoleId", "AccountId" },
                keyValues: new object[] { 1, "Admin" });

            migrationBuilder.DeleteData(
                table: "PersonalInformation",
                keyColumn: "AccountId",
                keyValue: "ADMIN");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: "ADMIN");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 1);
        }
    }
}
