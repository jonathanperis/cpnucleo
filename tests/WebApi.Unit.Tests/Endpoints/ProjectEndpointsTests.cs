namespace WebApi.Unit.Tests.Endpoints;

[TestFixture]
public class ProjectEndpointsTests
{
    [Test]
    public async Task GetProjectById_WithValidId_ShouldReturnProject()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var project = Project.Create("Test Project", organizationId, projectId);
        
        var fakeRepository = A.Fake<IProjectRepository>();
        A.CallTo(() => fakeRepository.GetByIdAsync(projectId))
            .Returns(Task.FromResult<Project?>(project));

        var ep = Factory.Create<WebApi.Endpoints.Project.GetProjectById.Endpoint>(fakeRepository);
        var req = new WebApi.Endpoints.Project.GetProjectById.Request { Id = projectId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Project.ShouldNotBeNull();
        ep.Response.Project.Id.ShouldBe(projectId);
        ep.Response.Project.Name.ShouldBe("Test Project");
    }

    [Test]
    public async Task GetProjectById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IProjectRepository>();
        A.CallTo(() => fakeRepository.GetByIdAsync(projectId))
            .Returns(Task.FromResult<Project?>(null));

        var ep = Factory.Create<WebApi.Endpoints.Project.GetProjectById.Endpoint>(fakeRepository);
        var req = new WebApi.Endpoints.Project.GetProjectById.Request { Id = projectId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task CreateProject_WithValidData_ShouldCreateProject()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var project = Project.Create("New Project", organizationId, projectId);
        
        var fakeRepository = A.Fake<IProjectRepository>();
        A.CallTo(() => fakeRepository.ExistsAsync(projectId)).Returns(Task.FromResult(false));
        A.CallTo(() => fakeRepository.AddAsync(A<Project>._)).Returns(Task.FromResult(projectId));
        A.CallTo(() => fakeRepository.GetByIdAsync(projectId)).Returns(Task.FromResult<Project?>(project));

        var ep = Factory.Create<WebApi.Endpoints.Project.CreateProject.Endpoint>(fakeRepository);
        var req = new WebApi.Endpoints.Project.CreateProject.Request
        {
            Id = projectId,
            Name = "New Project",
            OrganizationId = organizationId
        };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.ValidationFailed.ShouldBeFalse();
        ep.Response.ShouldNotBeNull();
        ep.Response.Project.ShouldNotBeNull();
        ep.Response.Project.Name.ShouldBe("New Project");
    }

    [Test]
    public async Task CreateProject_WithExistingId_ShouldFailValidation()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IProjectRepository>();
        A.CallTo(() => fakeRepository.ExistsAsync(projectId)).Returns(Task.FromResult(true));

        var ep = Factory.Create<WebApi.Endpoints.Project.CreateProject.Endpoint>(fakeRepository);
        var req = new WebApi.Endpoints.Project.CreateProject.Request
        {
            Id = projectId,
            Name = "New Project",
            OrganizationId = organizationId
        };

        // Act & Assert
        await Should.ThrowAsync<ValidationFailureException>(() => ep.HandleAsync(req, default));
    }

    [Test]
    public async Task UpdateProject_WithValidData_ShouldUpdateProject()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var project = Project.Create("Original Project", organizationId, projectId);
        
        var fakeRepository = A.Fake<IProjectRepository>();
        A.CallTo(() => fakeRepository.GetByIdAsync(projectId)).Returns(Task.FromResult<Project?>(project));
        A.CallTo(() => fakeRepository.UpdateAsync(A<Project>._)).Returns(Task.FromResult(true));

        var ep = Factory.Create<WebApi.Endpoints.Project.UpdateProject.Endpoint>(fakeRepository);
        var req = new WebApi.Endpoints.Project.UpdateProject.Request
        {
            Id = projectId,
            Name = "Updated Project",
            OrganizationId = organizationId
        };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Success.ShouldBeTrue();
    }

    [Test]
    public async Task UpdateProject_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IProjectRepository>();
        A.CallTo(() => fakeRepository.GetByIdAsync(projectId)).Returns(Task.FromResult<Project?>(null));

        var ep = Factory.Create<WebApi.Endpoints.Project.UpdateProject.Endpoint>(fakeRepository);
        var req = new WebApi.Endpoints.Project.UpdateProject.Request
        {
            Id = projectId,
            Name = "Updated Project",
            OrganizationId = organizationId
        };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task RemoveProject_WithValidId_ShouldDeleteProject()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var project = Project.Create("Project to Delete", organizationId, projectId);
        
        var fakeRepository = A.Fake<IProjectRepository>();
        A.CallTo(() => fakeRepository.GetByIdAsync(projectId)).Returns(Task.FromResult<Project?>(project));
        A.CallTo(() => fakeRepository.UpdateAsync(A<Project>._)).Returns(Task.FromResult(true));

        var ep = Factory.Create<WebApi.Endpoints.Project.RemoveProject.Endpoint>(fakeRepository);
        var req = new WebApi.Endpoints.Project.RemoveProject.Request { Ids = new List<Guid> { projectId } };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Success.ShouldBeTrue();
    }

    [Test]
    public async Task RemoveProject_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IProjectRepository>();
        A.CallTo(() => fakeRepository.GetByIdAsync(projectId)).Returns(Task.FromResult<Project?>(null));

        var ep = Factory.Create<WebApi.Endpoints.Project.RemoveProject.Endpoint>(fakeRepository);
        var req = new WebApi.Endpoints.Project.RemoveProject.Request { Ids = new List<Guid> { projectId } };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task ListProjects_ShouldReturnPaginatedResults()
    {
        // Arrange
        var projectId1 = Guid.NewGuid();
        var projectId2 = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var project1 = Project.Create("Project 1", organizationId, projectId1);
        var project2 = Project.Create("Project 2", organizationId, projectId2);
        
        var paginatedResult = new PaginatedResult<Project?>
        {
            Data = new List<Project?> { project1, project2 },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        var fakeRepository = A.Fake<IProjectRepository>();
        A.CallTo(() => fakeRepository.GetAllAsync(A<PaginationParams>._))
            .Returns(Task.FromResult(paginatedResult));

        var ep = Factory.Create<WebApi.Endpoints.Project.ListProjects.Endpoint>(fakeRepository);
        
        // Initialize response manually due to required property
        ep.Response = new WebApi.Endpoints.Project.ListProjects.Response 
        { 
            Result = new PaginatedResult<WebApi.Common.Dtos.ProjectDto?> 
            { 
                Data = new List<WebApi.Common.Dtos.ProjectDto?>(), 
                TotalCount = 0, 
                PageNumber = 1, 
                PageSize = 10 
            } 
        };
        
        var req = new WebApi.Endpoints.Project.ListProjects.Request
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
        rsp.Result.PageNumber.ShouldBe(1);
        rsp.Result.PageSize.ShouldBe(10);
    }
}
