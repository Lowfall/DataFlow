using DataFlow.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FileData> FilesData { get; set; }
        public DbSet<LoadedFile> LoadedFiles { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<FinancialClass> FinancialClasses { get; set; }

        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FinancialClass>()
                .HasMany(e => e.BankAccounts)
                .WithOne(e => e.FinancialClass)
                .HasForeignKey(e => e.FinancialClassId)
                .IsRequired();

            modelBuilder.Entity<LoadedFile>()
                .HasMany(e => e.FinancialClasses)
                .WithOne(e => e.LoadedFile)
                .HasForeignKey(e => e.LoadedFileId)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
