using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using HouseBrokerMVP.Core.Utilities;

namespace HouseBrokerMVP.Core.Context
{
    public partial class AppDbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
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
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["DeletedDate"] = GeneralUtility.GetCurrentDateTime();
                        }
                        break;
                    case EntityState.Modified:
                        if (HasProperty(entry.Entity, "ModifiedDate"))
                        {
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["ModifiedDate"] = GeneralUtility.GetCurrentDateTime();
                        }
                        break;
                    case EntityState.Added:
                        if (HasProperty(entry.Entity, "CreatedDate"))
                        {
                            entry.State = EntityState.Added;
                            entry.CurrentValues["CreatedDate"] = GeneralUtility.GetCurrentDateTime();
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
