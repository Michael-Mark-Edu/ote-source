using Microsoft.EntityFrameworkCore.Storage;

namespace OTE.Data.Test;

public class Tests : IDisposable
{
    private OteContextFactory _factory = null!;
    private OteContext _context = null!;
    private SchoolRepo _repo = null!;
    private IDbContextTransaction _transaction = null!;

    public Tests()
    {
        _factory = new OteContextFactory();
        _context = _factory.CreateDbContext([]);
        _repo = new SchoolRepo(_context, new MockLambdaLogger());
        _transaction = _context.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _transaction.Rollback();
    }

    [Fact]
    public void SetupTest()
    {
        if (_repo == null)
            Assert.Fail("_repo is null");
    }

    [Fact]
    public async Task BasicCRUDTest()
    {
        var all = await _repo.GetAll();
        Assert.NotNull(all);
        var initialCount = all.Count();

        var dto = new SchoolDto
        {
            SchoolName = "Foo Bar",
            SchoolAcronym = "FB",
            State = "OR",
            City = "Nowhere"
        };

        var entity = dto.Map();

        var insertedEntry = await _repo.Insert(entity);
        Assert.NotNull(insertedEntry);
        var inserted = insertedEntry.Entity;
        Assert.Equal(entity.SchoolName, inserted.SchoolName);
        Assert.Equal(entity.SchoolAcronym, inserted.SchoolAcronym);
        Assert.Equal(entity.State, inserted.State);
        Assert.Equal(entity.City, inserted.City);

        var key = inserted.SchoolID;

        all = await _repo.GetAll();
        Assert.NotNull(all);
        Assert.Equal(initialCount + 1, all.Count());

        dto = new SchoolDto
        {
            SchoolName = "Baz Quz",
            SchoolAcronym = "BQ",
            State = "OR",
            City = "Nowhere"
        };

        entity = dto.Map();

        var updatedEntry = await _repo.Update(key, entity);
        Assert.NotNull(updatedEntry);
        var updated = updatedEntry.Entity;
        Assert.Equal(entity.SchoolName, updated.SchoolName);
        Assert.Equal(entity.SchoolAcronym, updated.SchoolAcronym);
        Assert.Equal(entity.State, updated.State);
        Assert.Equal(entity.City, updated.City);

        all = await _repo.GetAll();
        Assert.NotNull(all);
        Assert.Equal(initialCount + 1, all.Count());

        var deletedEntry = await _repo.Delete(key);
        Assert.NotNull(deletedEntry);
        var deleted = deletedEntry.Entity;

        all = await _repo.GetAll();
        Assert.NotNull(all);
        Assert.Equal(initialCount, all.Count());
    }
}

