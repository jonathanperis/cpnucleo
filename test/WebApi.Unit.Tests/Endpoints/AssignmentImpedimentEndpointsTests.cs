using FakeItEasy;
using FastEndpoints;
using Shouldly;
using Domain.UoW;
using Domain.Repositories;
using Domain.Models;
using Domain.Entities;

namespace WebApi.Unit.Tests.Endpoints;

[TestFixture]
public class AssignmentImpedimentEndpointsTests
{
    [Test]
    public async Task GetAssignmentImpedimentById_WithValidId_ShouldReturnAssignmentImpediment()
    {
        // Arrange
        var assignmentImpedimentId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        var impedimentId = Guid.NewGuid();
        var assignmentImpediment = AssignmentImpediment.Create("Test Description", assignmentId, impedimentId, assignmentImpedimentId);
        
        var fakeRepository = A.Fake<IRepository<AssignmentImpediment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(assignmentImpedimentId))
            .Returns(Task.FromResult<AssignmentImpediment?>(assignmentImpediment));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<AssignmentImpediment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.AssignmentImpediment.GetAssignmentImpedimentById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.AssignmentImpediment.GetAssignmentImpedimentById.Request { Id = assignmentImpedimentId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.AssignmentImpediment.ShouldNotBeNull();
        ep.Response.AssignmentImpediment.Id.ShouldBe(assignmentImpedimentId);
        ep.Response.AssignmentImpediment.Description.ShouldBe("Test Description");
    }

    [Test]
    public async Task GetAssignmentImpedimentById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var assignmentImpedimentId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IRepository<AssignmentImpediment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(assignmentImpedimentId))
            .Returns(Task.FromResult<AssignmentImpediment?>(null));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<AssignmentImpediment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.AssignmentImpediment.GetAssignmentImpedimentById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.AssignmentImpediment.GetAssignmentImpedimentById.Request { Id = assignmentImpedimentId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task ListAssignmentImpediments_ShouldReturnPaginatedResults()
    {
        // Arrange
        var assignmentImpedimentId1 = Guid.NewGuid();
        var assignmentImpedimentId2 = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        var impedimentId = Guid.NewGuid();
        var assignmentImpediment1 = AssignmentImpediment.Create("Description 1", assignmentId, impedimentId, assignmentImpedimentId1);
        var assignmentImpediment2 = AssignmentImpediment.Create("Description 2", assignmentId, impedimentId, assignmentImpedimentId2);
        
        var paginatedResult = new PaginatedResult<AssignmentImpediment?>
        {
            Data = new List<AssignmentImpediment?> { assignmentImpediment1, assignmentImpediment2 },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        var fakeRepository = A.Fake<IRepository<AssignmentImpediment>>();
        A.CallTo(() => fakeRepository.GetAllAsync(A<PaginationParams>._))
            .Returns(Task.FromResult(paginatedResult));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<AssignmentImpediment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.AssignmentImpediment.ListAssignmentImpediments.Endpoint>(fakeUnitOfWork);
        
        // Initialize response manually due to required property
        ep.Response = new WebApi.Endpoints.AssignmentImpediment.ListAssignmentImpediments.Response 
        { 
            Result = new PaginatedResult<WebApi.Common.Dtos.AssignmentImpedimentDto?> 
            { 
                Data = new List<WebApi.Common.Dtos.AssignmentImpedimentDto?>(), 
                TotalCount = 0, 
                PageNumber = 1, 
                PageSize = 10 
            } 
        };
        
        var req = new WebApi.Endpoints.AssignmentImpediment.ListAssignmentImpediments.Request
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
