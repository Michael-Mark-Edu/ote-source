using Microsoft.EntityFrameworkCore.Storage;

namespace OTE.Data.Test;

[Collection("Database Tests")]
public class UserTests : IDisposable
{
    private OteContextFactory _factory = null!;
    private OteContext _context = null!;
    private UserRepo _repo = null!;
    private IDbContextTransaction _transaction = null!;

    public UserTests()
    {
        _factory = new OteContextFactory();
        _context = _factory.CreateDbContext();
        _repo = new UserRepo(_context);
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
        var all = (await _repo.GetAll()).Unwrap();
        var initialCount = all.Count();

        var schoolRepo = new SchoolRepo(_context);
        var passwordRepo = new Argon2idPasswordRepo(_context);

        var school = new SchoolEntity
        {
            SchoolId = 0,
            Name = "Foo Bar",
            Acronym = "FB",
            State = "OR",
            City = "Nowhere"
        };
        var schoolTracked = (await schoolRepo.Insert(school)).Unwrap();

        var password = new Argon2idPasswordEntity
        {
            Argon2idPasswordId = 0,
            Version = 10,
            MemoryCost = 65536,
            Iterations = 1,
            Parallelism = 4,
            Salt = new byte[] { 1, 2, 3, 4 },
            Hash = new byte[] { 5, 6, 7, 8 }
        };
        var passwordTracked = (await passwordRepo.Insert(password)).Unwrap();

        var dto = new UserDto
        {
            Username = "michael.mark",
            EmailAddress = "michael.mark@oit.edu",
            FirstName = "Michael",
            LastName = "Mark",
            MiddleName = "Lee Scott",
            SchoolId = schoolTracked.Entity.SchoolId,
            Argon2idPasswordId = passwordTracked.Entity.Argon2idPasswordId
        };

        var entity = dto.Map();

        var insertedEntry = (await _repo.Insert(entity)).Unwrap();
        var inserted = insertedEntry.Entity;
        Assert.Equal(entity.Username, inserted.Username);
        Assert.Equal(entity.EmailAddress, inserted.EmailAddress);
        Assert.Equal(entity.CreatedAt, inserted.CreatedAt);
        Assert.Equal(entity.FirstName, inserted.FirstName);
        Assert.Equal(entity.LastName, inserted.LastName);
        Assert.Equal(entity.MiddleName, inserted.MiddleName);
        Assert.NotNull(inserted.School);
        Assert.Equal(entity.SchoolId, inserted.SchoolId);
        Assert.NotNull(inserted.Argon2idPassword);
        Assert.Equal(entity.Argon2idPasswordId, inserted.Argon2idPasswordId);

        var key = inserted.UserId;

        all = (await _repo.GetAll()).Unwrap();
        Assert.NotNull(all);
        Assert.Equal(initialCount + 1, all.Count());

        var firstId = all.First().UserId;
        var first = (await _repo.FindById(firstId)).Unwrap();
        Assert.NotNull(first);
        Assert.Equal(firstId, first.UserId);

        dto = new UserDto
        {
            Username = "MichaelMark",
            EmailAddress = "michaelmark.education@gmail.com",
            FirstName = "Michael",
            LastName = "Mark",
            MiddleName = "Lee Scott",
            SchoolId = schoolTracked.Entity.SchoolId,
            Argon2idPasswordId = passwordTracked.Entity.Argon2idPasswordId
        };

        entity = dto.Map();

        var updatedEntry = (await _repo.Update(key, entity)).Unwrap();
        Assert.NotNull(updatedEntry);
        var updated = updatedEntry.Entity;
        Assert.Equal(entity.Username, updated.Username);
        Assert.Equal(entity.EmailAddress, updated.EmailAddress);
        Assert.Equal(entity.CreatedAt, updated.CreatedAt);
        Assert.Equal(entity.FirstName, updated.FirstName);
        Assert.Equal(entity.LastName, updated.LastName);
        Assert.Equal(entity.MiddleName, updated.MiddleName);
        Assert.NotNull(updated.School);
        Assert.Equal(entity.SchoolId, updated.SchoolId);
        Assert.NotNull(updated.Argon2idPassword);
        Assert.Equal(entity.Argon2idPasswordId, updated.Argon2idPasswordId);

        all = (await _repo.GetAll()).Unwrap();
        Assert.NotNull(all);
        Assert.Equal(initialCount + 1, all.Count());

        var deletedEntry = (await _repo.Delete(key)).Unwrap();
        Assert.NotNull(deletedEntry);
        var deleted = deletedEntry.Entity;

        all = (await _repo.GetAll()).Unwrap();
        Assert.NotNull(all);
        Assert.Equal(initialCount, all.Count());
    }
}

