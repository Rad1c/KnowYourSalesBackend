using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MODEL.Entities;

namespace MODEL.Interceptors;

public class DbSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntitiesWithAdditionalData(eventData.Context!);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntitiesWithAdditionalData(eventData.Context!);
        return ValueTask.FromResult(result);
    }

    /// <summary>
    /// Updates the audit fields of added or modified entities and attaches any Enumeration entities to the context.
    /// </summary>
    /// <param name="context">The DbContext instance to use for the update.</param>
    /// <remarks>
    /// Fill additional fields of BasEnitiy object with data related to user who changed/added object and date of changeing/adding object 
    /// for all objects from context.ChangeTracker that have state Modified or Added. It also attaches any entities
    /// of type Enumeration to the context because they are included in the database.
    /// </remarks>
    private void UpdateEntitiesWithAdditionalData(DbContext context)
    {
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity changedObject && changedObject is not null)
            {
                if (entry.State == EntityState.Added)
                {
                    changedObject = UpdateAddedEntity(changedObject, context);
                }
                else if (entry.State == EntityState.Modified)
                {
                    changedObject = UpdateEditedEntity(changedObject, context);
                }
                else
                {
                    entry.State = EntityState.Modified;
                    changedObject = UpdateDeletedEntity(changedObject, context);
                }
            }
        }
    }

    private T UpdateAddedEntity<T>(T entity, DbContext context) where T : BaseEntity
    {
        context.Entry(entity).Property(x => x.Created).CurrentValue = DateTime.UtcNow;

        return entity;
    }

    private T UpdateEditedEntity<T>(T entity, DbContext context) where T : BaseEntity
    {
        context.Entry(entity).Property(x => x.Modified).CurrentValue = DateTime.UtcNow;

        return entity;
    }

    /// <summary>
    /// Updates the specified entity by setting its "Invalidated" property to true and updating its "Updated", "UpdatedByUser", "UpdatedIP", and "Version" properties.
    /// </summary>
    private T UpdateDeletedEntity<T>(T entity, DbContext context) where T : BaseEntity
    {
        context.Entry(entity).Property(x => x.IsDeleted).CurrentValue = true;
        context.Entry(entity).Property("modified").CurrentValue = DateTime.UtcNow;

        return entity;
    }
}
