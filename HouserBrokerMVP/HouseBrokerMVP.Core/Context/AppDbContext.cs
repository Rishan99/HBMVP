using HouseBrokerMVP.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Core.Context
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Property> Properties { get; set; } = null!;
        public virtual DbSet<PropertyImage> PropertyImages { get; set; } = null!;
        public virtual DbSet<PropertyType> PropertyTypes { get; set; } = null!;
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("name=DefaultConnection");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IdentityUser>(e =>
            {
                e.HasKey(x => x.Id);

            });
            modelBuilder.Entity<IdentityUserRole<string>>(e => e.HasKey(r => new { r.UserId, r.RoleId }));
            modelBuilder.Entity<IdentityUserLogin<string>>(e => e.HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId }));
            modelBuilder.Entity<IdentityUserToken<string>>(e => e.HasKey(t => new { t.UserId, t.LoginProvider, t.Name }));
            modelBuilder.Entity<PropertyType>();
            modelBuilder.Entity<PropertyImage>(e =>
            {
                e.HasOne(d => d.Property).WithMany(d => d.PropertyImages).HasForeignKey(d => d.PropertyId);
            });
            modelBuilder.Entity<Property>(e =>
            {
                e.HasOne<PropertyType>(d => d.PropertyType).WithMany(d => d.Properties).HasForeignKey(d => d.PropertyTypeId);
                e.HasOne(d => d.Broker).WithMany(d => d.Properties).HasForeignKey(d => d.BrokerId);
            }
            );
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
