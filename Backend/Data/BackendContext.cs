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
        public DbSet<Backend.Models.ClazzSubject> ClazzSubject { get; set; }

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


        public DbSet<Backend.Models.Mark> Mark { get; set; }

        public DbSet<Backend.Models.Subject> Subject { get; set; }
    }
}