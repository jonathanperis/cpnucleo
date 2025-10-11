using FakeItEasy;
using FastEndpoints;
using Shouldly;
using Domain.UoW;
using Domain.Repositories;
using Domain.Models;
using Domain.Entities;

namespace WebApi.Unit.Tests.Endpoints;

[TestFixture]
public class AssignmentTypeEndpointsTests
{
    [Test]
    public async Task GetAssignmentTypeById_WithValidId_ShouldReturnAssignmentType()
    {
        // Arrange
        var assignmentTypeId = Guid.NewGuid();
        var assignmentType = AssignmentType.Create("Test Type", assignmentTypeId);
        
        var fakeRepository = A.Fake<IRepository<AssignmentType>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(assignmentTypeId))
            .Returns(Task.FromResult<AssignmentType?>(assignmentType));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<AssignmentType>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.AssignmentType.GetAssignmentTypeById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.AssignmentType.GetAssignmentTypeById.Request { Id = assignmentTypeId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.AssignmentType.ShouldNotBeNull();
        ep.Response.AssignmentType.Id.ShouldBe(assignmentTypeId);
        ep.Response.AssignmentType.Name.ShouldBe("Test Type");
    }

    [Test]
    public async Task GetAssignmentTypeById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var assignmentTypeId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IRepository<AssignmentType>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(assignmentTypeId))
            .Returns(Task.FromResult<AssignmentType?>(null));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<AssignmentType>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.AssignmentType.GetAssignmentTypeById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.AssignmentType.GetAssignmentTypeById.Request { Id = assignmentTypeId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task ListAssignmentTypes_ShouldReturnPaginatedResults()
    {
        // Arrange
        var assignmentTypeId1 = Guid.NewGuid();
        var assignmentTypeId2 = Guid.NewGuid();
        var assignmentType1 = AssignmentType.Create("Type 1", assignmentTypeId1);
        var assignmentType2 = AssignmentType.Create("Type 2", assignmentTypeId2);
        
        var paginatedResult = new PaginatedResult<AssignmentType?>
        {
            Data = new List<AssignmentType?> { assignmentType1, assignmentType2 },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        var fakeRepository = A.Fake<IRepository<AssignmentType>>();
        A.CallTo(() => fakeRepository.GetAllAsync(A<PaginationParams>._))
            .Returns(Task.FromResult(paginatedResult));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<AssignmentType>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.AssignmentType.ListAssignmentTypes.Endpoint>(fakeUnitOfWork);
        
        // Initialize response manually due to required property
        ep.Response = new WebApi.Endpoints.AssignmentType.ListAssignmentTypes.Response 
        { 
            Result = new PaginatedResult<WebApi.Common.Dtos.AssignmentTypeDto?> 
            { 
                Data = new List<WebApi.Common.Dtos.AssignmentTypeDto?>(), 
                TotalCount = 0, 
                PageNumber = 1, 
                PageSize = 10 
            } 
        };
        
        var req = new WebApi.Endpoints.AssignmentType.ListAssignmentTypes.Request
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
