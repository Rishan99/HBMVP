using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Enum;
using HouseBrokerMVP.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace HouseBrokerMVP.Core.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Property> Properties { get; set; } = null!;
        public virtual DbSet<PropertyImage> PropertyImages { get; set; } = null!;
        public virtual DbSet<PropertyType> PropertyTypes { get; set; } = null!;
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
     : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer("name=DefaultConnection");
        //}

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
            modelCreatingConfiguration(modelBuilder);
            SeedDefaultValues(modelBuilder);
        }

        private void SeedDefaultValues(ModelBuilder modelBuilder)
        {
            SeedRoleValues(modelBuilder);
        }

        private void SeedRoleValues(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "1",
                    Name = nameof(RoleEnum.SuperAdmin),
                    NormalizedName = nameof(RoleEnum.SuperAdmin).ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                 new IdentityRole()
                 {
                     Id = "2",
                     Name = nameof(RoleEnum.HouseSeeker),
                     NormalizedName = nameof(RoleEnum.HouseSeeker).ToUpper(),
                     ConcurrencyStamp = Guid.NewGuid().ToString()
                 },
                 new IdentityRole()
                 {
                     Id = "3",
                     Name = nameof(RoleEnum.Broker),
                     NormalizedName = nameof(RoleEnum.Broker).ToUpper(),
                     ConcurrencyStamp = Guid.NewGuid().ToString()
                 }
                );

        }
        private void modelCreatingConfiguration(ModelBuilder modelBuilder)
        {

            var temp = modelBuilder.Model.GetEntityTypes().ToList();
            foreach (var entityType in temp)
            {
                var deletedDateProperty = entityType.FindProperty("DeletedDate");
                if (deletedDateProperty != null && deletedDateProperty.ClrType == typeof(DateTime?))
                {
                    var parameter = Expression.Parameter(entityType.ClrType);
                    var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(DateTime?));
                    var deletedDatePropertyExp = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("DeletedDate"));
                    BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, deletedDatePropertyExp, Expression.Constant(null));
                    var lambda = Expression.Lambda(compareExpression, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {

            ApplySoftDelete();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplySoftDelete();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplySoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        if (HasProperty(entry.Entity, "DeletedDate"))
                        {
                            entry.CurrentValues["DeletedDate"] = GeneralUtility.GetCurrentDateTime();
                        }
                        break;
                    case EntityState.Modified:
                        if (HasProperty(entry.Entity, "ModifiedDate"))
                        {
                            entry.CurrentValues["ModifiedDate"] = GeneralUtility.GetCurrentDateTime();
                            // temporary TODO
                            entry.CurrentValues["ModifiedBy"] = "admin";
                        }
                        break;
                    case EntityState.Added:
                        if (HasProperty(entry.Entity, "CreatedDate"))
                        {
                            entry.CurrentValues["CreatedDate"] = GeneralUtility.GetCurrentDateTime();
                            // temporary TODO
                            entry.CurrentValues["CreatedBy"] = "admin";
                        }
                        break;
                }
            }
        }
        private static bool HasProperty(object obj, string propertyName)
        {
            Type type = obj.GetType();
            return type.GetProperty(propertyName) != null;
        }
    }
}
