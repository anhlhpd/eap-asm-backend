using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecurityHandle;
using Backend.Models;

namespace Backend.Models
{
    public class BackendContext : DbContext
    {
        public BackendContext (DbContextOptions<BackendContext> options)
            : base(options)
        {
        }

        public DbSet<Backend.Models.StudentClass> StudentClass { get; set; }
        public DbSet<Backend.Models.PersonalInformation> PersonalInformation { get; set; }
        public DbSet<Backend.Models.Account> Account { get; set; }
        public DbSet<Backend.Models.Role> Role { get; set; }

        public DbSet<Backend.Models.Credential> Credential { get; set; }
        public DbSet<Backend.Models.AccountRole> AccountRoles { get; set; }
        public DbSet<Backend.Models.StudentClassAccount> StudentClassAccount { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountRole>()
                .HasKey(ar => new { ar.AccountId, ar.RoleId });

            modelBuilder.Entity<Account>(entity => {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });
            modelBuilder.Entity<Role>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
            modelBuilder.Entity<StudentClass>(entity => {
                entity.HasIndex(e => new { e.Session, e.StartDate }).IsUnique();
            });
            modelBuilder.Entity<Subject>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
            modelBuilder.Entity<Credential>(entity => {
                entity.HasIndex(e => e.AccessToken).IsUnique();
            });
            modelBuilder.Entity<PersonalInformation>(entity => {
                entity.HasIndex(e => e.Phone).IsUnique();
            });
            modelBuilder.Entity<AccountRole>(entity => {
                entity.HasKey(e => new { e.RoleId, e.AccountId });
            });
            modelBuilder.Entity<StudentClassAccount>(entity => {
                entity.HasKey(e => new { e.StudentClassId, e.AccountId });
            });

            //Seeder for First Admin
            var salt = PasswordHandle.GetInstance().GenerateSalt();
            
            modelBuilder.Entity<PersonalInformation>().HasData(new PersonalInformation()
            {
                AccountId = "ADMIN",
                FirstName = "ADMIN",
                LastName = "ADMIN",
                Phone = "01234567890",
            });
            
            modelBuilder.Entity<Account>().HasData(new Account()
            {
                AccountId = "ADMIN",
                Username = "ADMIN",
                Salt = salt,
                Password = PasswordHandle.GetInstance().EncryptPassword("Amin@123", salt),
                Email = "admin@admin.com",
            });

            modelBuilder.Entity<Role>().HasData(new Role()
            {
                RoleId = 1,
                Name = "Admin"
            });

            modelBuilder.Entity<AccountRole>().HasData(new AccountRole()
            {
                AccountId = "Admin",
                RoleId = 1,
            });

            //Seeder for 2 teachers and 2 students
            // Teachers
            salt = PasswordHandle.GetInstance().GenerateSalt();
            modelBuilder.Entity<Account>().HasData(new Account()
            {
                AccountId = "TCH0001",
                Username = "xuanhung24",
                Salt = salt,
                Password = PasswordHandle.GetInstance().EncryptPassword("A123@a123", salt),
                Email = "xuanhung2401@gmail.com",
            });
            modelBuilder.Entity<PersonalInformation>().HasData(new PersonalInformation()
            {
                AccountId = "TCH0001",
                FirstName = "Hung",
                LastName = "Dao",
                Phone = "013237416",
            });
            salt = PasswordHandle.GetInstance().GenerateSalt();
            modelBuilder.Entity<Account>().HasData(new Account()
            {
                AccountId = "TCH0002",
                Username = "hongluyen",
                Salt = salt,
                Password = PasswordHandle.GetInstance().EncryptPassword("A123@a123", salt),
                Email = "hongluyen@gmail.com",
            });
            modelBuilder.Entity<PersonalInformation>().HasData(new PersonalInformation()
            {
                AccountId = "TCH0002",
                FirstName = "Luyen",
                LastName = "Dao",
                Phone = "013257416",
            });
            modelBuilder.Entity<Role>().HasData(new Role()
            {
                RoleId = 2,
                Name = "Teacher"
            });
            modelBuilder.Entity<AccountRole>().HasData(new AccountRole()
            {
                AccountId = "TCH0001",
                RoleId = 2,
            });
            modelBuilder.Entity<AccountRole>().HasData(new AccountRole()
            {
                AccountId = "TCH0002",
                RoleId = 2,
            });
            // Students
            salt = PasswordHandle.GetInstance().GenerateSalt();
            modelBuilder.Entity<Account>().HasData(new Account()
            {
                AccountId = "STU0001",
                Username = "thuthao541998",
                Salt = salt,
                Password = PasswordHandle.GetInstance().EncryptPassword("A123@a123", salt),
                Email = "thuthao541998@gmail.com",
            });
            modelBuilder.Entity<PersonalInformation>().HasData(new PersonalInformation()
            {
                AccountId = "STU0001",
                FirstName = "Thao",
                LastName = "Nguyen",
                Phone = "013257983",
            });
            salt = PasswordHandle.GetInstance().GenerateSalt();
            modelBuilder.Entity<Account>().HasData(new Account()
            {
                AccountId = "STU0002",
                Username = "anhnhpd00579",
                Salt = salt,
                Password = PasswordHandle.GetInstance().EncryptPassword("A123@a123", salt),
                Email = "anhnhpd00579@fpt.edu.vn",
            });
            modelBuilder.Entity<PersonalInformation>().HasData(new PersonalInformation()
            {
                AccountId = "STU0002",
                FirstName = "Anh",
                LastName = "Nguyen",
                Phone = "0130387983",
            });

            modelBuilder.Entity<Role>().HasData(new Role()
            {
                RoleId = 3,
                Name = "Student"
            });
            modelBuilder.Entity<AccountRole>().HasData(new AccountRole()
            {
                AccountId = "STU0002",
                RoleId = 3,
            });
            modelBuilder.Entity<AccountRole>().HasData(new AccountRole()
            {
                AccountId = "STU0001",
                RoleId = 3,
            });
        }
    }
}