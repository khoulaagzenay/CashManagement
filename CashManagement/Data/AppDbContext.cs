using CashManagement.Models.DTOs.Roles;
using CashManagement.Models.Entities;
using CashManagement.Models.Entities.Authentication;
using CashManagement.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace CashManagement.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<DailyBalance> DailyBalances { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public static async Task seedRoles(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
               if(! await roleManager.RoleExistsAsync(UserRolesDto.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (!await roleManager.RoleExistsAsync(UserRolesDto.User))
                {
                    await roleManager.CreateAsync(new IdentityRole("User"));
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<DailyBalance>().HasKey(d => d.Id); 

        //Account → Transactions
        modelBuilder.Entity<Transaction>()
    .HasOne(t => t.Account)
    .WithMany(a => a.Transactions)
    .HasForeignKey(t => t.AccountId);

            //Account → DailyBalances
            modelBuilder.Entity<DailyBalance>()
    .HasOne(db => db.Account)
    .WithMany(a => a.DailyBalances)
    .HasForeignKey(db => db.AccountId);
            //Account → Alerts (optionnelle)
            modelBuilder.Entity<Alert>()
    .HasOne(a => a.Account)
    .WithMany()
    .HasForeignKey(a => a.AccountId)
    .IsRequired(false);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
