using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Repositories;

/// <summary>`Argon2idPasswordEntity` implementation of `AbstractRepo`.</summary>
/// <param name="context">The `OteContext` the repository uses.</param>
public class Argon2idPasswordRepo(OteContext context) : AbstractRepo<Argon2idPasswordEntity>(context)
{
    protected override IQueryable<Argon2idPasswordEntity> _queryable {
        get
        {
            return _dbSet
                .Where(e => e.DeletedAt == null);
        }
    }
}
