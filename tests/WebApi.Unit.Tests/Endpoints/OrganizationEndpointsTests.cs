namespace WebApi.Unit.Tests.Endpoints;

[TestFixture]
public class OrganizationEndpointsTests
{
    [Test]
    public async Task GetOrganizationById_WithValidId_ShouldReturnOrganization()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var organization = Organization.Create("Test Organization", "Test Description", organizationId);
        
        var fakeRepository = A.Fake<IRepository<Organization>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(organizationId))
            .Returns(Task.FromResult<Organization?>(organization));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Organization>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Organization.GetOrganizationById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Organization.GetOrganizationById.Request { Id = organizationId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Organization.ShouldNotBeNull();
        ep.Response.Organization.Id.ShouldBe(organizationId);
        ep.Response.Organization.Name.ShouldBe("Test Organization");
    }

    [Test]
    public async Task GetOrganizationById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IRepository<Organization>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(organizationId))
            .Returns(Task.FromResult<Organization?>(null));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Organization>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Organization.GetOrganizationById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Organization.GetOrganizationById.Request { Id = organizationId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task CreateOrganization_WithValidData_ShouldCreateOrganization()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var organization = Organization.Create("New Organization", "New Description", organizationId);
        
        var fakeRepository = A.Fake<IRepository<Organization>>();
        A.CallTo(() => fakeRepository.ExistsAsync(organizationId)).Returns(Task.FromResult(false));
        A.CallTo(() => fakeRepository.AddAsync(A<Organization>._)).Returns(Task.FromResult(organizationId));
        A.CallTo(() => fakeRepository.GetByIdAsync(organizationId)).Returns(Task.FromResult<Organization?>(organization));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Organization>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Organization.CreateOrganization.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Organization.CreateOrganization.Request
        {
            Id = organizationId,
            Name = "New Organization",
            Description = "New Description"
        };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.ValidationFailed.ShouldBeFalse();
        ep.Response.ShouldNotBeNull();
        ep.Response.Organization.ShouldNotBeNull();
        ep.Response.Organization.Name.ShouldBe("New Organization");
    }

    [Test]
    public async Task UpdateOrganization_WithValidData_ShouldUpdateOrganization()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var organization = Organization.Create("Original Organization", "Original Description", organizationId);
        
        var fakeRepository = A.Fake<IRepository<Organization>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(organizationId)).Returns(Task.FromResult<Organization?>(organization));
        A.CallTo(() => fakeRepository.UpdateAsync(A<Organization>._)).Returns(Task.FromResult(true));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Organization>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Organization.UpdateOrganization.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Organization.UpdateOrganization.Request
        {
            Id = organizationId,
            Name = "Updated Organization",
            Description = "Updated Description"
        };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Success.ShouldBeTrue();
    }

    [Test]
    public async Task RemoveOrganization_WithValidId_ShouldDeleteOrganization()
    {
        // Arrange
        var organizationId = Guid.NewGuid();
        var organization = Organization.Create("Organization to Delete", "Description", organizationId);
        
        var fakeRepository = A.Fake<IRepository<Organization>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(organizationId)).Returns(Task.FromResult<Organization?>(organization));
        A.CallTo(() => fakeRepository.UpdateAsync(A<Organization>._)).Returns(Task.FromResult(true));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Organization>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Organization.RemoveOrganization.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Organization.RemoveOrganization.Request { Ids = new List<Guid> { organizationId } };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Success.ShouldBeTrue();
    }

    [Test]
    public async Task ListOrganizations_ShouldReturnPaginatedResults()
    {
        // Arrange
        var organizationId1 = Guid.NewGuid();
        var organizationId2 = Guid.NewGuid();
        var organization1 = Organization.Create("Organization 1", "Description 1", organizationId1);
        var organization2 = Organization.Create("Organization 2", "Description 2", organizationId2);
        
        var paginatedResult = new PaginatedResult<Organization?>
        {
            Data = new List<Organization?> { organization1, organization2 },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        var fakeRepository = A.Fake<IRepository<Organization>>();
        A.CallTo(() => fakeRepository.GetAllAsync(A<PaginationParams>._))
            .Returns(Task.FromResult(paginatedResult));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Organization>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Organization.ListOrganizations.Endpoint>(fakeUnitOfWork);
        
        // Initialize response manually due to required property
        ep.Response = new WebApi.Endpoints.Organization.ListOrganizations.Response 
        { 
            Result = new PaginatedResult<WebApi.Common.Dtos.OrganizationDto?> 
            { 
                Data = new List<WebApi.Common.Dtos.OrganizationDto?>(), 
                TotalCount = 0, 
                PageNumber = 1, 
                PageSize = 10 
            } 
        };
        
        var req = new WebApi.Endpoints.Organization.ListOrganizations.Request
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
