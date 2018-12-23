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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountRole>()
                .HasKey(ar => new { ar.AccountId, ar.RoleId });
            //Seeder for First Admin
            var salt = PasswordHandle.GetInstance().GenerateSalt();
            modelBuilder.Entity<Account>().HasData(new Account()
            {
                AccountId = "ADMIN",
                Username = "ADMIN",
                Salt = salt,
                Password = PasswordHandle.GetInstance().EncryptPassword("Amin@123",salt),
                Email = "admin@admin.com",
            });
            modelBuilder.Entity<PersonalInformation>().HasData(new PersonalInformation()
            {
                AccountId = "ADMIN",
                FirstName = "ADMIN",
                LastName = "ADMIN",
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

        public DbSet<Backend.Models.Account> Account { get; set; }

        public DbSet<Backend.Models.Role> Role { get; set; }

        public DbSet<Backend.Models.Credential> Credential { get; set; }
        public DbSet<Backend.Models.AccountRole> AccountRoles { get; set; }
        
    } 
}
