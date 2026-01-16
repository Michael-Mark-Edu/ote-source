using Microsoft.EntityFrameworkCore.Storage;

namespace OTE.Data.Test;

public class Argon2idPasswordTests : IDisposable
{
    private OteContextFactory _factory = null!;
    private OteContext _context = null!;
    private Argon2idPasswordRepo _repo = null!;
    private IDbContextTransaction _transaction = null!;

    public Argon2idPasswordTests()
    {
        _factory = new OteContextFactory();
        _context = _factory.CreateDbContext(["test"]);
        _repo = new Argon2idPasswordRepo(_context, new MockLambdaLogger());
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

        var dto = new Argon2idPasswordDto
        {
            Version = 10,
            MemoryCost = 65536,
            Iterations = 1,
            Parallelism = 4,
            Salt = new byte[] { 1, 2, 3, 4 },
            Hash = new byte[] { 5, 6, 7, 8 }
        };

        var entity = dto.Map(null);

        var insertedEntry = await _repo.Insert(entity);
        Assert.NotNull(insertedEntry);
        var inserted = insertedEntry.Entity;
        Assert.Equal(entity.Version, inserted.Version);
        Assert.Equal(entity.MemoryCost, inserted.MemoryCost);
        Assert.Equal(entity.Iterations, inserted.Iterations);
        Assert.Equal(entity.Parallelism, inserted.Parallelism);
        Assert.Equal(entity.Salt, inserted.Salt);
        Assert.Equal(entity.Hash, inserted.Hash);

        var key = inserted.Argon2idPasswordId;

        all = await _repo.GetAll();
        Assert.NotNull(all);
        Assert.Equal(initialCount + 1, all.Count());

        dto = new Argon2idPasswordDto
        {
            Version = 10,
            MemoryCost = 65536,
            Iterations = 8,
            Parallelism = 8,
            Salt = new byte[] { 10, 20, 30, 40 },
            Hash = new byte[] { 50, 60, 70, 80 }
        };

        entity = dto.Map(null);

        var updatedEntry = await _repo.Update(key, entity);
        Assert.NotNull(updatedEntry);
        var updated = updatedEntry.Entity;
        Assert.Equal(entity.Version, updated.Version);
        Assert.Equal(entity.MemoryCost, updated.MemoryCost);
        Assert.Equal(entity.Iterations, updated.Iterations);
        Assert.Equal(entity.Parallelism, updated.Parallelism);
        Assert.Equal(entity.Salt, updated.Salt);
        Assert.Equal(entity.Hash, updated.Hash);

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

