namespace Integration.Tests.Modules;

[Collection("Scenarios"), Order(5)]
public class UserModuleTests(WebAppFixture fixture)
{
    private readonly IAlbaHost _host = fixture.AlbaHost;
    private readonly Guid _userId = fixture.UserId;
    
    [Fact, Order(21)]
    public async Task Users_ShouldCreateAnUser()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Post.Json(new CreateUserCommand(
                $"Integration Test User #{DateTime.UtcNow.Ticks.ToString()}",
                $"$integration_test_user_{DateTime.UtcNow.Ticks.ToString()}",
                "SuperSecurePasswordBelieveIt", 
                _userId)).ToUrl("/api/users");
            s.StatusCodeShouldBe(201);
        });
    }    
    
    [Fact, Order(22)]
    public async Task Users_ShouldReturnAllUsers()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new PaginationParams { PageNumber = 1, PageSize = 10, SortColumn = "Id", SortOrder = "ASC"}).ToUrl("/api/users");
            s.StatusCodeShouldBeOk();
        });
    }    
    
    [Fact, Order(23)]
    public async Task Users_ShouldGetAnUser()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Get.Json(new{ }).ToUrl("/api/users/" + _userId);
            s.StatusCodeShouldBe(200);
        });      
    }
    
    [Fact, Order(24)]
    public async Task Users_ShouldUpdateAnUser()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Patch.Json(new UpdateUserCommand(_userId,
                $"Integration Test User UPDATED #{DateTime.UtcNow.Ticks.ToString()}",
                "StillSuperSecurePasswordBelieveIt")).ToUrl("/api/users/" + _userId);
            s.StatusCodeShouldBe(204);
        });
    }
    
    [Fact, Order(25)]
    public async Task Users_ShouldDeleteAnUser()
    {
        // Arrange //Act // Assert
        await _host.Scenario(s =>
        {
            s.Delete.Json(new{ }).ToUrl("/api/users/" + _userId);
            s.StatusCodeShouldBe(204);
        });    
    }
}