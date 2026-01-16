using Amazon.Lambda.Core;
using Microsoft.EntityFrameworkCore;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Repositories;

/// <summary>`UserEntity` implementation of `AbstractRepo`.</summary>
/// <param name="context">The `OteContext` the repository uses.</param>
/// <param name="logger">The `ILambdaLogger` used for logging.</param>
public class UserRepo(OteContext context, ILambdaLogger logger) : AbstractRepo<UserEntity>(context, logger)
{
    protected override IQueryable<UserEntity> _queryable {
        get
        {
            return _dbSet
                .Include(d => d.School)
                .Include(d => d.Argon2idPassword);
        }
    }
}
