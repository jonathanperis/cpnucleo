using Infrastructure.Common.Context;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Unit.Tests.Endpoints;

[TestFixture]
public class UserEndpointsTests
{
    [Test]
    public async Task GetUserById_WithValidId_ShouldReturnUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create("Test User", "testuser", new PasswordHash("hash-value", string.Empty), userId);
        
        var fakeRepository = A.Fake<IRepository<User>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(userId))
            .Returns(Task.FromResult<User?>(user));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<User>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.User.GetUserById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.User.GetUserById.Request { Id = userId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.User.ShouldNotBeNull();
        ep.Response.User.Id.ShouldBe(userId);
        ep.Response.User.Name.ShouldBe("Test User");
    }

    [Test]
    public async Task GetUserById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IRepository<User>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(userId))
            .Returns(Task.FromResult<User?>(null));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<User>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.User.GetUserById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.User.GetUserById.Request { Id = userId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    [Ignore("EF Core DbSet.Any() extension method cannot be mocked with FakeItEasy. Use integration tests for EF Core-based endpoints.")]
    public async Task CreateUser_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create("New User", "newuser", new PasswordHash("hash-value", string.Empty), userId);
        
        var fakeDbContext = A.Fake<IApplicationDbContext>();
        var fakeDbSet = A.Fake<DbSet<User>>();
        
        A.CallTo(() => fakeDbContext.Users).Returns(fakeDbSet);
        A.CallTo(() => fakeDbSet.Any(A<System.Linq.Expressions.Expression<Func<User, bool>>>._)).Returns(false);
        A.CallTo(() => fakeDbContext.SaveChangesAsync(A<CancellationToken>._)).Returns(true);
        A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._, A<CancellationToken>._)).Returns(new ValueTask<User?>(user));

        var passwordHasher = A.Fake<IPasswordHasher>();
        A.CallTo(() => passwordHasher.Hash("Password@123")).Returns(new PasswordHash("$argon2id$hash", string.Empty));

        var ep = Factory.Create<WebApi.Endpoints.User.CreateUser.Endpoint>(fakeDbContext, passwordHasher);
        var req = new WebApi.Endpoints.User.CreateUser.Request
        {
            Id = userId,
            Name = "New User",
            Login = "newuser",
            Password = "Password@123"
        };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.ValidationFailed.ShouldBeFalse();
        ep.Response.ShouldNotBeNull();
        ep.Response.User.ShouldNotBeNull();
        ep.Response.User.Name.ShouldBe("New User");
    }

    [Test]
    public async Task UpdateUser_WithValidData_ShouldUpdateUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create("Original User", "originaluser", new PasswordHash("hash-value", string.Empty), userId);
        
        var fakeDbContext = A.Fake<IApplicationDbContext>();
        var fakeDbSet = A.Fake<DbSet<User>>();
        
        A.CallTo(() => fakeDbContext.Users).Returns(fakeDbSet);
        A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._, A<CancellationToken>._)).Returns(new ValueTask<User?>(user));
        A.CallTo(() => fakeDbContext.SaveChangesAsync(A<CancellationToken>._)).Returns(true);

        var passwordHasher = A.Fake<IPasswordHasher>();
        A.CallTo(() => passwordHasher.Hash("Password@123")).Returns(new PasswordHash("$argon2id$updated-hash", string.Empty));

        var ep = Factory.Create<WebApi.Endpoints.User.UpdateUser.Endpoint>(fakeDbContext, passwordHasher);
        var req = new WebApi.Endpoints.User.UpdateUser.Request
        {
            Id = userId,
            Name = "Updated User",
            Password = "Password@123"
        };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Success.ShouldBeTrue();
        A.CallTo(() => passwordHasher.Hash("Password@123")).MustHaveHappenedOnceExactly();
        user.Password.ShouldBe("$argon2id$updated-hash");
        user.Salt.ShouldBeEmpty();
    }

    [Test]
    public async Task RemoveUser_WithValidId_ShouldDeleteUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create("User to Delete", "deleteuser", new PasswordHash("hash-value", string.Empty), userId);
        
        var fakeDbContext = A.Fake<IApplicationDbContext>();
        var fakeDbSet = A.Fake<DbSet<User>>();
        
        A.CallTo(() => fakeDbContext.Users).Returns(fakeDbSet);
        A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._, A<CancellationToken>._)).Returns(new ValueTask<User?>(user));
        A.CallTo(() => fakeDbContext.SaveChangesAsync(A<CancellationToken>._)).Returns(true);

        var ep = Factory.Create<WebApi.Endpoints.User.RemoveUser.Endpoint>(fakeDbContext);
        var req = new WebApi.Endpoints.User.RemoveUser.RemoveUserRequest { Ids = new List<Guid> { userId } };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Success.ShouldBeTrue();
    }

    [Test]
    public async Task ListUsers_ShouldReturnPaginatedResults()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var user1 = User.Create("User 1", "user1", new PasswordHash("hash-value-1", string.Empty), userId1);
        var user2 = User.Create("User 2", "user2", new PasswordHash("hash-value-2", string.Empty), userId2);
        
        var paginatedResult = new PaginatedResult<User?>
        {
            Data = new List<User?> { user1, user2 },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        var fakeRepository = A.Fake<IRepository<User>>();
        A.CallTo(() => fakeRepository.GetAllAsync(A<PaginationParams>._))
            .Returns(Task.FromResult(paginatedResult));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<User>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.User.ListUsers.Endpoint>(fakeUnitOfWork);
        
        // Initialize response manually due to required property
        ep.Response = new WebApi.Endpoints.User.ListUsers.Response 
        { 
            Result = new PaginatedResult<WebApi.Common.Dtos.UserDto?> 
            { 
                Data = new List<WebApi.Common.Dtos.UserDto?>(), 
                TotalCount = 0, 
                PageNumber = 1, 
                PageSize = 10 
            } 
        };
        
        var req = new WebApi.Endpoints.User.ListUsers.Request
        {
            Pagination = new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC" }
        };

        // Act
        await ep.HandleAsync(req, default);
        var rsp = ep.Response;

        // Assert
        rsp.ShouldNotBeNull();
        rsp.Result.ShouldNotBeNull();
        rsp.Result.Data.ShouldNotBeNull();
        rsp.Result.Data.Count().ShouldBe(2);
        rsp.Result.TotalCount.ShouldBe(2);
    }
}
