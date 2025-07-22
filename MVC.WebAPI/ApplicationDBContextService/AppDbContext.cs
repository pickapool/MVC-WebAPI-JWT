using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Services.ApplicationDBContextService
{
    public class AppDbContext : IdentityDbContext<ApplicationUserModel>
    {
        public DbSet<TokenInfoModel> TokenInfos { get; set; }
        public DbSet<PatientModel> Patients { get; set; }
        public DbSet<PatientMedicineModel> PatientMedicines { get; set; }
        public DbSet<MedicineModel> Medicines { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Rename Identity tables
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            // Rename your own table
            builder.Entity<ApplicationUserModel>().ToTable("Users");
            builder.Entity<TokenInfoModel>().ToTable("TokenInfos");

            builder.Entity<PatientModel>().ToTable("Patients");
            builder.Entity<PatientMedicineModel>().ToTable("PatientMedicines");
            builder.Entity<MedicineModel>().ToTable("Medicines");
        }
    }
}

