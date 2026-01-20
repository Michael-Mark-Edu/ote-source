using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;
using OTE.Common;
using OTE.Data.EFCore.Contexts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OTE.Data.EFCore.Repositories;

/// <summary>Abstract class that implements virtual repository methods.</summary>
/// <param name="context">The `OteContext` to use for database CRUD.</param>
/// <typeparam name="TEntity">The entity type the repository uses.</typeparam>
public abstract class AbstractRepo<TEntity>(OteContext context)
    where TEntity : class
{
    protected DbSet<TEntity> _dbSet = context.Set<TEntity>();

    protected virtual IQueryable<TEntity> _queryable {
        get
        {
            return _dbSet.AsQueryable();
        }
    }

    /// <summary>Gets all entities in the table.</summary>
    /// <returns>The entities in the table, or an `Exception` if this fails.</returns>
    public virtual async Task<Result<IEnumerable<TEntity>, NpgsqlException>> GetAll()
    {
        try
        {
            return new(await _queryable.ToListAsync());
        }
        catch (NpgsqlException e)
        {
            return new(e);
        }
        catch (Exception e)
        {
            for (Exception? i = e; i != null; i = i.InnerException)
            {
                if (i.GetType().IsAssignableTo(typeof(NpgsqlException)))
                    return new ((NpgsqlException)i);
            }
            throw;
        }
    }

    /// <summary>Inserts an entity into the table.</summary>
    /// <param name="entity">The `TEntity` containing the data to insert into the table.</param>
    /// <returns>A tracking entry of the inserted entity, or an `Exception` if this fails.</returns>
    public virtual async ValueTask<Result<EntityEntry<TEntity>, NpgsqlException>> Insert(TEntity entity)
    {
        try
        {
            var entry = await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return new(entry);
        }
        catch (Exception e)
        {
            for (Exception? i = e; i != null; i = i.InnerException)
            {
                if (i.GetType().IsAssignableTo(typeof(NpgsqlException)))
                    return new ((NpgsqlException)i);
            }
            throw;
        }
    }

    /// <summary>Updates an entity in the table.</summary>
    /// <param name="key">The primary key of the table entity that you want to update.</param>
    /// <param name="entity">The `TEntity` containing the data to replace the table entity with.</param>
    /// <returns>A tracking entry of the updated entity, or null if no object with the key exists, or an `Exception` if this fails.</returns>
    public virtual async Task<Result<EntityEntry<TEntity>?, NpgsqlException>> Update(object key, TEntity entity)
    {
        try
        {
            TEntity? target = await _dbSet.FindAsync(key);
            if (target == null)
                return new((EntityEntry<TEntity>?)null);

            foreach (var property in typeof(TEntity).GetProperties())
            {
                if (property.GetCustomAttributes(typeof(KeyAttribute), false).Any())
                    continue;
                if (property.GetCustomAttributes(typeof(ForeignKeyAttribute), false).Any())
                    continue;

                property.SetValue(target, property.GetValue(entity));
            }

            var update = _dbSet.Update(target);
            await context.SaveChangesAsync();
            return new(update);
        }
        catch (Exception e)
        {
            for (Exception? i = e; i != null; i = i.InnerException)
            {
                if (i.GetType().IsAssignableTo(typeof(NpgsqlException)))
                    return new ((NpgsqlException)i);
            }
            throw;
        }
    }

    /// <summary>Deletes an entity in the table.</summary>
    /// <param name="key">The primary key of the table entity that you want to delete.</param>
    /// <returns>A tracking entry of the deleted entity, or null if no object with the key exists, or an `Exception` if this fails.</returns>
    public virtual async Task<Result<EntityEntry<TEntity>?, NpgsqlException>> Delete(object key)
    {
        try
        {
            TEntity? dbEntity = await _dbSet.FindAsync(key);
            if (dbEntity == null)
                return new((EntityEntry<TEntity>?)null);

            var removed = _dbSet.Remove(dbEntity);
            await context.SaveChangesAsync();
            return new(removed);
        }
        catch (Exception e)
        {
            for (Exception? i = e; i != null; i = i.InnerException)
            {
                if (i.GetType().IsAssignableTo(typeof(NpgsqlException)))
                    return new ((NpgsqlException)i);
            }
            throw;
        }
    }
}
