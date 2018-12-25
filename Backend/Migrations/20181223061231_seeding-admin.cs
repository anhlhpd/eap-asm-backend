using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class SeedingAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "AccountId", "AccountStatus", "CreatedAt", "DeletedAt", "Email", "Password", "Salt", "UpdatedAt", "Username" },
                values: new object[] { "ADMIN", 1, new DateTime(2018, 12, 23, 13, 12, 31, 613, DateTimeKind.Local), new DateTime(2018, 12, 23, 13, 12, 31, 615, DateTimeKind.Local), "admin@admin.com", "bD1wo9lWUhncdeNzKpriLXOapoCdqmQzXBdV//UkNFs=", "8LK0MsWDvA4sawsyU+PotA==", new DateTime(2018, 12, 23, 13, 12, 31, 615, DateTimeKind.Local), "ADMIN" });

            migrationBuilder.InsertData(
                table: "PersonalInformation",
                columns: new[] { "AccountId", "Birthday", "FirstName", "Gender", "LastName", "Phone" },
                values: new object[] { "ADMIN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN", 2, "ADMIN", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PersonalInformation",
                keyColumn: "AccountId",
                keyValue: "ADMIN");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountId",
                keyValue: "ADMIN");
        }
    }
}
