using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Repositories;

/// <summary>`SchoolEntity` implementation of `AbstractRepo`.</summary>
/// <param name="context">The `OteContext` the repository uses.</param>
public class SchoolRepo(OteContext context) : AbstractRepo<SchoolEntity>(context)
{
    protected override IQueryable<SchoolEntity> _queryable {
        get
        {
            return _dbSet
                .Where(e => e.DeletedAt == null);
        }
    }
}
