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

        public DbSet<Backend.Models.Clazz> Clazz { get; set; }
        public DbSet<Backend.Models.GeneralInformation> GeneralInformation { get; set; }
        public DbSet<Backend.Models.Account> Account { get; set; }
        public DbSet<Backend.Models.Role> Role { get; set; }

        public DbSet<Backend.Models.Credential> Credential { get; set; }
        public DbSet<Backend.Models.AccountRole> AccountRoles { get; set; }
        public DbSet<Backend.Models.ClazzAccount> ClazzAccount { get; set; }

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
            modelBuilder.Entity<Clazz>(entity => {
                entity.HasIndex(e => new { e.Session, e.StartDate }).IsUnique();
            });
            modelBuilder.Entity<Subject>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
            modelBuilder.Entity<Credential>(entity => {
                entity.HasIndex(e => e.AccessToken).IsUnique();
            });
            modelBuilder.Entity<GeneralInformation>(entity => {
                entity.HasIndex(e => e.Phone).IsUnique();
            });
            modelBuilder.Entity<AccountRole>(entity => {
                entity.HasKey(e => new { e.RoleId, e.AccountId });
            });
            modelBuilder.Entity<ClazzAccount>(entity => {
                entity.HasKey(e => new { e.ClazzId, e.AccountId });
            });

            //Seeder for First Admin
            var salt = PasswordHandle.GetInstance().GenerateSalt();
            
            modelBuilder.Entity<GeneralInformation>().HasData(new GeneralInformation()
            {
                AccountId = "ADMIN",
                FirstName = "ADMIN",
                LastName = "ADMIN",
                Phone = "01234567890",
            });
            
            modelBuilder.Entity<Account>().HasData(new Account()
            {
                Id = "ADMIN",
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
        }
    }
}