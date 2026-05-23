namespace WebApi.Unit.Tests.Endpoints;

[TestFixture]
public class AssignmentEndpointsTests
{
    [Test]
    public async Task GetAssignmentById_WithValidId_ShouldReturnAssignment()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var workflowId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var assignmentTypeId = Guid.NewGuid();
        var assignment = Assignment.Create("Test Assignment", "Test Description", DateTime.UtcNow, DateTime.UtcNow.AddDays(7), 40, projectId, workflowId, userId, assignmentTypeId, assignmentId);
        
        var fakeRepository = A.Fake<IRepository<Assignment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(assignmentId))
            .Returns(Task.FromResult<Assignment?>(assignment));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Assignment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Assignment.GetAssignmentById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Assignment.GetAssignmentById.Request { Id = assignmentId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Assignment.ShouldNotBeNull();
        ep.Response.Assignment.Id.ShouldBe(assignmentId);
        ep.Response.Assignment.Name.ShouldBe("Test Assignment");
    }

    [Test]
    public async Task GetAssignmentById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IRepository<Assignment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(assignmentId))
            .Returns(Task.FromResult<Assignment?>(null));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Assignment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Assignment.GetAssignmentById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Assignment.GetAssignmentById.Request { Id = assignmentId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task ListAssignments_ShouldReturnPaginatedResults()
    {
        // Arrange
        var assignmentId1 = Guid.NewGuid();
        var assignmentId2 = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var workflowId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var assignmentTypeId = Guid.NewGuid();
        var assignment1 = Assignment.Create("Assignment 1", "Description 1", DateTime.UtcNow, DateTime.UtcNow.AddDays(7), 40, projectId, workflowId, userId, assignmentTypeId, assignmentId1);
        var assignment2 = Assignment.Create("Assignment 2", "Description 2", DateTime.UtcNow, DateTime.UtcNow.AddDays(7), 40, projectId, workflowId, userId, assignmentTypeId, assignmentId2);
        
        var paginatedResult = new PaginatedResult<Assignment?>
        {
            Data = new List<Assignment?> { assignment1, assignment2 },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        var fakeRepository = A.Fake<IRepository<Assignment>>();
        A.CallTo(() => fakeRepository.GetAllAsync(A<PaginationParams>._))
            .Returns(Task.FromResult(paginatedResult));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Assignment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Assignment.ListAssignments.Endpoint>(fakeUnitOfWork);
        
        // Initialize response manually due to required property
        ep.Response = new WebApi.Endpoints.Assignment.ListAssignments.Response 
        { 
            Result = new PaginatedResult<WebApi.Common.Dtos.AssignmentDto?> 
            { 
                Data = new List<WebApi.Common.Dtos.AssignmentDto?>(), 
                TotalCount = 0, 
                PageNumber = 1, 
                PageSize = 10 
            } 
        };
        
        var req = new WebApi.Endpoints.Assignment.ListAssignments.Request
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
