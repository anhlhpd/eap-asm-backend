﻿// <auto-generated />
using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Migrations
{
    [DbContext(typeof(BackendContext))]
    [Migration("20181225125238_initialize")]
    partial class initialize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Backend.Models.Account", b =>
                {
                    b.Property<string>("AccountId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountStatus");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Salt");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("AccountId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Account");

                    b.HasData(
                        new { AccountId = "ADMIN", AccountStatus = 1, CreatedAt = new DateTime(2018, 12, 25, 19, 52, 37, 894, DateTimeKind.Local), DeletedAt = new DateTime(2018, 12, 25, 19, 52, 37, 895, DateTimeKind.Local), Email = "admin@admin.com", Password = "xFibEz32G9f7mfMF3YDP2dHeLlOiSVodL+LD9eSHsks=", Salt = "UsbuqtyJbmlCTki5UjOh9Q==", UpdatedAt = new DateTime(2018, 12, 25, 19, 52, 37, 895, DateTimeKind.Local), Username = "ADMIN" }
                    );
                });

            modelBuilder.Entity("Backend.Models.AccountRole", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<string>("AccountId");

                    b.HasKey("RoleId", "AccountId");

                    b.HasIndex("AccountId");

                    b.ToTable("AccountRole");
                });

            modelBuilder.Entity("Backend.Models.Credential", b =>
                {
                    b.Property<string>("AccountId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken");

                    b.Property<string>("AccountId1");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("AccountId");

                    b.HasIndex("AccessToken")
                        .IsUnique()
                        .HasFilter("[AccessToken] IS NOT NULL");

                    b.HasIndex("AccountId1");

                    b.ToTable("Credential");
                });

            modelBuilder.Entity("Backend.Models.PersonalInformation", b =>
                {
                    b.Property<string>("AccountId");

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int>("Gender");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.HasKey("AccountId");

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("PersonalInformation");

                    b.HasData(
                        new { AccountId = "ADMIN", Birthday = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), FirstName = "ADMIN", Gender = 2, LastName = "ADMIN", Phone = "0" }
                    );
                });

            modelBuilder.Entity("Backend.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("RoleStatus");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("RoleId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Backend.Models.StudentClass", b =>
                {
                    b.Property<string>("StudentClassId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CurrentSubjectId");

                    b.Property<string>("Session")
                        .IsRequired();

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("StudentClassStatus");

                    b.Property<int?>("SubjectId");

                    b.HasKey("StudentClassId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("Session", "StartDate")
                        .IsUnique();

                    b.ToTable("StudentClass");
                });

            modelBuilder.Entity("Backend.Models.StudentClassAccount", b =>
                {
                    b.Property<string>("StudentClassId");

                    b.Property<string>("AccountId");

                    b.HasKey("StudentClassId", "AccountId");

                    b.HasIndex("AccountId");

                    b.ToTable("StudentClassAccount");
                });

            modelBuilder.Entity("Backend.Models.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("SubjectStatus");

                    b.HasKey("SubjectId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("Backend.Models.AccountRole", b =>
                {
                    b.HasOne("Backend.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Backend.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Backend.Models.Credential", b =>
                {
                    b.HasOne("Backend.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId1");
                });

            modelBuilder.Entity("Backend.Models.PersonalInformation", b =>
                {
                    b.HasOne("Backend.Models.Account", "Account")
                        .WithOne("PersonalInformation")
                        .HasForeignKey("Backend.Models.PersonalInformation", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Backend.Models.StudentClass", b =>
                {
                    b.HasOne("Backend.Models.Subject", "Subject")
                        .WithMany("StudentClasses")
                        .HasForeignKey("SubjectId");
                });

            modelBuilder.Entity("Backend.Models.StudentClassAccount", b =>
                {
                    b.HasOne("Backend.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Backend.Models.StudentClass", "StudentClass")
                        .WithMany()
                        .HasForeignKey("StudentClassId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}