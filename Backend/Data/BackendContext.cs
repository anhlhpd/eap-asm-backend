using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class BackendContext : DbContext
    {
        public BackendContext (DbContextOptions<BackendContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity => {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });
            modelBuilder.Entity<Role>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
            modelBuilder.Entity<StudentClass>(entity => {
                entity.HasIndex(e => new { e.Session, e.StartDate}).IsUnique();
            });
            modelBuilder.Entity<Subject>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
            modelBuilder.Entity<Credential>(entity => {
                entity.HasIndex(e => e.AccessToken).IsUnique();
            });
            modelBuilder.Entity<PersonalInformation>(entity => {
                entity.HasIndex(e => new { e.FirstName, e.LastName, e.Phone }).IsUnique();
            });
        }

        public DbSet<Backend.Models.PersonalInformation> PersonalInformation { get; set; }
    }
}
