using Microsoft.EntityFrameworkCore.Storage;

namespace OTE.Data.Test;

public class UserTests : IDisposable
{
    private OteContextFactory _factory = null!;
    private OteContext _context = null!;
    private UserRepo _repo = null!;
    private IDbContextTransaction _transaction = null!;

    public UserTests()
    {
        _factory = new OteContextFactory();
        _context = _factory.CreateDbContext(["test"]);
        _repo = new UserRepo(_context, new MockLambdaLogger());
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

        var schoolRepo = new SchoolRepo(_context, new MockLambdaLogger());
        var passwordRepo = new Argon2idPasswordRepo(_context, new MockLambdaLogger());

        var school = new SchoolEntity
        {
            SchoolId = 0,
            Name = "Foo Bar",
            Acronym = "FB",
            State = "OR",
            City = "Nowhere"
        };
        var schoolTracked = await schoolRepo.Insert(school);
        Assert.NotNull(schoolTracked);

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
        var passwordTracked = await passwordRepo.Insert(password);
        Assert.NotNull(passwordTracked);

        var dto = new UserDto
        {
            FirstName = "Michael",
            LastName = "Mark",
            MiddleName = "Lee Scott",
            EmailAddress = "michael.mark@oit.edu",
            SchoolId = schoolTracked.Entity.SchoolId,
            Argon2idPasswordId = passwordTracked.Entity.Argon2idPasswordId
        };

        var entity = dto.Map([[schoolTracked.Entity], [passwordTracked.Entity]]);
        Assert.NotNull(entity);

        var insertedEntry = await _repo.Insert(entity);
        Assert.NotNull(insertedEntry);
        var inserted = insertedEntry.Entity;
        Assert.Equal(entity.FirstName, inserted.FirstName);
        Assert.Equal(entity.LastName, inserted.LastName);
        Assert.Equal(entity.MiddleName, inserted.MiddleName);
        Assert.Equal(entity.EmailAddress, inserted.EmailAddress);
        Assert.Equal(entity.School, inserted.School);
        Assert.Equal(entity.Argon2idPassword, inserted.Argon2idPassword);

        var key = inserted.UserId;

        all = await _repo.GetAll();
        Assert.NotNull(all);
        Assert.Equal(initialCount + 1, all.Count());

        dto = new UserDto
        {
            FirstName = "Michael",
            LastName = "Mark",
            MiddleName = "Lee Scott",
            EmailAddress = "michaelmark.education@gmail.com",
            SchoolId = schoolTracked.Entity.SchoolId,
            Argon2idPasswordId = passwordTracked.Entity.Argon2idPasswordId
        };

        entity = dto.Map([[schoolTracked.Entity], [passwordTracked.Entity]]);
        Assert.NotNull(entity);

        var updatedEntry = await _repo.Update(key, entity);
        Assert.NotNull(updatedEntry);
        var updated = updatedEntry.Entity;
        Assert.Equal(entity.FirstName, updated.FirstName);
        Assert.Equal(entity.LastName, updated.LastName);
        Assert.Equal(entity.MiddleName, updated.MiddleName);
        Assert.Equal(entity.EmailAddress, updated.EmailAddress);
        Assert.Equal(entity.School, updated.School);
        Assert.Equal(entity.Argon2idPassword, updated.Argon2idPassword);

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

