using Microsoft.EntityFrameworkCore.Storage;

namespace OTE.Data.Test;

public class SchoolTests : IDisposable
{
    private OteContextFactory _factory = null!;
    private OteContext _context = null!;
    private SchoolRepo _repo = null!;
    private IDbContextTransaction _transaction = null!;

    public SchoolTests()
    {
        _factory = new OteContextFactory();
        _context = _factory.CreateDbContext();
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
            Name = "Foo Bar",
            Acronym = "FB",
            State = "OR",
            City = "Nowhere"
        };

        var entity = dto.Map();

        var insertedEntry = await _repo.Insert(entity);
        Assert.NotNull(insertedEntry);
        var inserted = insertedEntry.Entity;
        Assert.Equal(entity.Name, inserted.Name);
        Assert.Equal(entity.Acronym, inserted.Acronym);
        Assert.Equal(entity.State, inserted.State);
        Assert.Equal(entity.City, inserted.City);

        var key = inserted.SchoolId;

        all = await _repo.GetAll();
        Assert.NotNull(all);
        Assert.Equal(initialCount + 1, all.Count());

        dto = new SchoolDto
        {
            Name = "Baz Quz",
            Acronym = "BQ",
            State = "OR",
            City = "Nowhere"
        };

        entity = dto.Map();

        var updatedEntry = await _repo.Update(key, entity);
        Assert.NotNull(updatedEntry);
        var updated = updatedEntry.Entity;
        Assert.Equal(entity.Name, updated.Name);
        Assert.Equal(entity.Acronym, updated.Acronym);
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

