using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    SubjectId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Credential",
                columns: table => new
                {
                    OwnerId = table.Column<string>(nullable: false),
                    AccessToken = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credential", x => x.OwnerId);
                    table.ForeignKey(
                        name: "FK_Credential_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneralInformation",
                columns: table => new
                {
                    AccountId = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 10, nullable: false),
                    LastName = table.Column<string>(maxLength: 10, nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralInformation", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_GeneralInformation_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    AccountId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRoles", x => new { x.RoleId, x.AccountId });
                    table.UniqueConstraint("AK_AccountRoles_AccountId_RoleId", x => new { x.AccountId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AccountRoles_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clazz",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Session = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CurrentSubjectId = table.Column<int>(nullable: false),
                    SubjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clazz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clazz_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClazzAccount",
                columns: table => new
                {
                    ClazzId = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClazzAccount", x => new { x.ClazzId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_ClazzAccount_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClazzAccount_Clazz_ClazzId",
                        column: x => x.ClazzId,
                        principalTable: "Clazz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "Password", "Salt", "Status", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { "ADMIN", new DateTime(2018, 12, 29, 11, 9, 32, 526, DateTimeKind.Local), null, "admin@admin.com", "2H8lp+baHG0SXIRZZM5135rqp29eB8YvMXfYtTeEx4A=", "W0j9N8yjoBOW50O1swZ32w==", 1, new DateTime(2018, 12, 29, 11, 9, 32, 526, DateTimeKind.Local), "ADMIN" },
                    { "MNG0001", new DateTime(2018, 12, 29, 11, 9, 32, 542, DateTimeKind.Local), null, "xuanhung2401@gmail.com", "RFi0m78QqNt/d75Yhmoxc7zY/p0PkV4Z4ysQ4HXFbIw=", "s0fvk3vhV3ZTHXmJ4AsoiA==", 1, new DateTime(2018, 12, 29, 11, 9, 32, 542, DateTimeKind.Local), "xuanhung24" },
                    { "MNG0002", new DateTime(2018, 12, 29, 11, 9, 32, 553, DateTimeKind.Local), null, "hongluyen@gmail.com", "jy6T7QOqLzawvBwF9RRN4XNe+AMjyL+ykDHolrr6Vvw=", "PbFmJtQuisi/PBHxwXAUaQ==", 1, new DateTime(2018, 12, 29, 11, 9, 32, 553, DateTimeKind.Local), "hongluyen" },
                    { "STU0001", new DateTime(2018, 12, 29, 11, 9, 32, 564, DateTimeKind.Local), null, "thuthao541998@gmail.com", "ZYPOlDKhfrehLheeciVDfFA1yJy8hh4Em1dTJT+0kBc=", "B0xkN1xyjJEplGtHpseynw==", 1, new DateTime(2018, 12, 29, 11, 9, 32, 564, DateTimeKind.Local), "thuthao541998" },
                    { "STU0002", new DateTime(2018, 12, 29, 11, 9, 32, 576, DateTimeKind.Local), null, "anhnhpd00579@fpt.edu.vn", "6lB1xhARb41H8QWhRuwDDZ956LPVpIujWUOKFyr4p0c=", "INxZuId19JjR7zIddVowVw==", 1, new DateTime(2018, 12, 29, 11, 9, 32, 576, DateTimeKind.Local), "anhnhpd00579" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "CreatedAt", "DeletedAt", "Description", "Name", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2018, 12, 29, 11, 9, 32, 520, DateTimeKind.Local), new DateTime(2018, 12, 29, 11, 9, 32, 522, DateTimeKind.Local), "Set role for Admin User", "Admin", 1, new DateTime(2018, 12, 29, 11, 9, 32, 522, DateTimeKind.Local) },
                    { 2, new DateTime(2018, 12, 29, 11, 9, 32, 523, DateTimeKind.Local), new DateTime(2018, 12, 29, 11, 9, 32, 523, DateTimeKind.Local), "Set role for Manage User", "Manage", 1, new DateTime(2018, 12, 29, 11, 9, 32, 523, DateTimeKind.Local) },
                    { 3, new DateTime(2018, 12, 29, 11, 9, 32, 523, DateTimeKind.Local), new DateTime(2018, 12, 29, 11, 9, 32, 523, DateTimeKind.Local), "Set role for Student User", "Student", 1, new DateTime(2018, 12, 29, 11, 9, 32, 523, DateTimeKind.Local) }
                });

            migrationBuilder.InsertData(
                table: "AccountRoles",
                columns: new[] { "RoleId", "AccountId" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "MNG0001" },
                    { 2, "MNG0002" },
                    { 3, "STU0002" },
                    { 3, "STU0001" }
                });

            migrationBuilder.InsertData(
                table: "GeneralInformation",
                columns: new[] { "AccountId", "Birthday", "FirstName", "Gender", "LastName", "Phone" },
                values: new object[,]
                {
                    { "ADMIN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN", 2, "ADMIN", "01234567890" },
                    { "MNG0001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hung", 2, "Dao", "013237416" },
                    { "MNG0002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luyen", 2, "Dao", "013257416" },
                    { "STU0001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thao", 2, "Nguyen", "013257983" },
                    { "STU0002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anh", 2, "Nguyen", "0130387983" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Username",
                table: "Account",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clazz_SubjectId",
                table: "Clazz",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Clazz_Session_StartDate",
                table: "Clazz",
                columns: new[] { "Session", "StartDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClazzAccount_AccountId",
                table: "ClazzAccount",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Credential_AccessToken",
                table: "Credential",
                column: "AccessToken",
                unique: true,
                filter: "[AccessToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Credential_AccountId",
                table: "Credential",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInformation_Phone",
                table: "GeneralInformation",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subject_Name",
                table: "Subject",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRoles");

            migrationBuilder.DropTable(
                name: "ClazzAccount");

            migrationBuilder.DropTable(
                name: "Credential");

            migrationBuilder.DropTable(
                name: "GeneralInformation");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Clazz");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
