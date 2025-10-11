using FakeItEasy;
using FastEndpoints;
using Shouldly;
using Domain.UoW;
using Domain.Repositories;
using Domain.Models;
using Domain.Entities;

namespace WebApi.Unit.Tests.Endpoints;

[TestFixture]
public class UserAssignmentEndpointsTests
{
    [Test]
    public async Task GetUserAssignmentById_WithValidId_ShouldReturnUserAssignment()
    {
        // Arrange
        var userAssignmentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        var userAssignment = UserAssignment.Create(userId, assignmentId, userAssignmentId);
        
        var fakeRepository = A.Fake<IRepository<UserAssignment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(userAssignmentId))
            .Returns(Task.FromResult<UserAssignment?>(userAssignment));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<UserAssignment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.UserAssignment.GetUserAssignmentById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.UserAssignment.GetUserAssignmentById.Request { Id = userAssignmentId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.UserAssignment.ShouldNotBeNull();
        ep.Response.UserAssignment.Id.ShouldBe(userAssignmentId);
    }

    [Test]
    public async Task GetUserAssignmentById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var userAssignmentId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IRepository<UserAssignment>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(userAssignmentId))
            .Returns(Task.FromResult<UserAssignment?>(null));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<UserAssignment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.UserAssignment.GetUserAssignmentById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.UserAssignment.GetUserAssignmentById.Request { Id = userAssignmentId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task ListUserAssignments_ShouldReturnPaginatedResults()
    {
        // Arrange
        var userAssignmentId1 = Guid.NewGuid();
        var userAssignmentId2 = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        var userAssignment1 = UserAssignment.Create(userId, assignmentId, userAssignmentId1);
        var userAssignment2 = UserAssignment.Create(userId, assignmentId, userAssignmentId2);
        
        var paginatedResult = new PaginatedResult<UserAssignment?>
        {
            Data = new List<UserAssignment?> { userAssignment1, userAssignment2 },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        var fakeRepository = A.Fake<IRepository<UserAssignment>>();
        A.CallTo(() => fakeRepository.GetAllAsync(A<PaginationParams>._))
            .Returns(Task.FromResult(paginatedResult));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<UserAssignment>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.UserAssignment.ListUserAssignments.Endpoint>(fakeUnitOfWork);
        
        // Initialize response manually due to required property
        ep.Response = new WebApi.Endpoints.UserAssignment.ListUserAssignments.Response 
        { 
            Result = new PaginatedResult<WebApi.Common.Dtos.UserAssignmentDto?> 
            { 
                Data = new List<WebApi.Common.Dtos.UserAssignmentDto?>(), 
                TotalCount = 0, 
                PageNumber = 1, 
                PageSize = 10 
            } 
        };
        
        var req = new WebApi.Endpoints.UserAssignment.ListUserAssignments.Request
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
