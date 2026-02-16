using Amazon.Lambda.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OTE.Data.EFCore.Contexts;
using System.ComponentModel.DataAnnotations;

namespace OTE.Data.EFCore.Repositories;

/// <summary>Abstract class that implements virtual repository methods.</summary>
/// <param name="context">The `OteContext` to use for database CRUD.</param>
/// <param name="logger">The `ILambdaLogger` used for logging.</param>
/// <typeparam name="TEntity">The entity type the repository uses.</typeparam>
public abstract class AbstractRepo<TEntity>(OteContext context, ILambdaLogger logger)
    where TEntity : class
{
    protected DbSet<TEntity> _dbSet = context.Set<TEntity>();

    /// <summary>Gets all entities in the table.</summary>
    /// <returns>The entities in the table.</returns>
    /// <remarks>Returns null if an exception occured.</remarks>
    public virtual async Task<IEnumerable<TEntity>?> GetAll()
    {
        try
        {
            return await _dbSet.ToListAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return null;
        }
    }

    /// <summary>Inserts an entity into the table.</summary>
    /// <param name="entity">The `TEntity` containing the data to insert into the table.</param>
    /// <returns>A tracking entry of the inserted entity.</returns>
    /// <remarks>Returns null if an exception occured.</remarks>
    public virtual async ValueTask<EntityEntry<TEntity>?> Insert(TEntity entity)
    {
        try
        {
            var entry = await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entry;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return null;
        }
    }

    /// <summary>Updates an entity in the table.</summary>
    /// <param name="key">The primary key of the table entity that you want to update.</param>
    /// <param name="entity">The `TEntity` containing the data to replace the table entity with.</param>
    /// <returns>A tracking entry of the updated entity.</returns>
    /// <remarks>Returns null if an exception occured, or if no objects with the key exist in the table.</remarks>
    public virtual async Task<EntityEntry<TEntity>?> Update(object key, TEntity entity)
    {
        try
        {
            TEntity? target = await _dbSet.FindAsync(key);
            if (target == null)
                return null;

            foreach (var property in typeof(TEntity).GetProperties())
            {
                if (property.GetCustomAttributes(typeof(KeyAttribute), false).Any())
                    continue;

                property.SetValue(target, property.GetValue(entity));
            }

            var update = _dbSet.Update(target);
            await context.SaveChangesAsync();
            return update;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return null;
        }
    }

    /// <summary>Deletes an entity in the table.</summary>
    /// <param name="key">The primary key of the table entity that you want to delete.</param>
    /// <returns>A tracking entry of the deleted entity.</returns>
    /// <remarks>Returns null if an exception occured, or if no objects with the key exist in the table.</remarks>
    public virtual async Task<EntityEntry<TEntity>?> Delete(object key)
    {
        try
        {
            TEntity? dbEntity = await _dbSet.FindAsync(key);
            if (dbEntity == null)
                return null;

            var removed = _dbSet.Remove(dbEntity);
            await context.SaveChangesAsync();
            return removed;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return null;
        }
    }
}
