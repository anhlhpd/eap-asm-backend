﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: false),
                    AccountStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
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
                    RoleStatus = table.Column<int>(nullable: false)
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
                    SubjectStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.SubjectId);
                });

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

            migrationBuilder.CreateTable(
                name: "PersonalInformation",
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
                    table.PrimaryKey("PK_PersonalInformation", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_PersonalInformation_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
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
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentClass",
                columns: table => new
                {
                    StudentClassId = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Session = table.Column<int>(nullable: false),
                    StudentClassStatus = table.Column<int>(nullable: false),
                    CurrentSubjectId = table.Column<int>(nullable: false),
                    SubjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClass", x => x.StudentClassId);
                    table.ForeignKey(
                        name: "FK_StudentClass_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentClassAccount",
                columns: table => new
                {
                    StudentClassId = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClassAccount", x => new { x.StudentClassId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_StudentClassAccount_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentClassAccount_StudentClass_StudentClassId",
                        column: x => x.StudentClassId,
                        principalTable: "StudentClass",
                        principalColumn: "StudentClassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_Username",
                table: "Account",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credential_AccessToken",
                table: "Credential",
                column: "AccessToken",
                unique: true,
                filter: "[AccessToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Credential_AccountId1",
                table: "Credential",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInformation_Phone",
                table: "PersonalInformation",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentClass_SubjectId",
                table: "StudentClass",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClass_Session_StartDate",
                table: "StudentClass",
                columns: new[] { "Session", "StartDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassAccount_AccountId",
                table: "StudentClassAccount",
                column: "AccountId");

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
                name: "Credential");

            migrationBuilder.DropTable(
                name: "PersonalInformation");

            migrationBuilder.DropTable(
                name: "StudentClassAccount");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "StudentClass");

            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}