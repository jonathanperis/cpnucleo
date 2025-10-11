using FakeItEasy;
using FastEndpoints;
using Shouldly;
using Domain.UoW;
using Domain.Repositories;
using Domain.Models;
using Domain.Entities;

namespace WebApi.Unit.Tests.Endpoints;

[TestFixture]
public class UserProjectEndpointsTests
{
    [Test]
    public async Task GetUserProjectById_WithValidId_ShouldReturnUserProject()
    {
        // Arrange
        var userProjectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var userProject = UserProject.Create(userId, projectId, userProjectId);
        
        var fakeRepository = A.Fake<IRepository<UserProject>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(userProjectId))
            .Returns(Task.FromResult<UserProject?>(userProject));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<UserProject>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.UserProject.GetUserProjectById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.UserProject.GetUserProjectById.Request { Id = userProjectId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.UserProject.ShouldNotBeNull();
        ep.Response.UserProject.Id.ShouldBe(userProjectId);
    }

    [Test]
    public async Task GetUserProjectById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var userProjectId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IRepository<UserProject>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(userProjectId))
            .Returns(Task.FromResult<UserProject?>(null));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<UserProject>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.UserProject.GetUserProjectById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.UserProject.GetUserProjectById.Request { Id = userProjectId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    public async Task ListUserProjects_ShouldReturnPaginatedResults()
    {
        // Arrange
        var userProjectId1 = Guid.NewGuid();
        var userProjectId2 = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var userProject1 = UserProject.Create(userId, projectId, userProjectId1);
        var userProject2 = UserProject.Create(userId, projectId, userProjectId2);
        
        var paginatedResult = new PaginatedResult<UserProject?>
        {
            Data = new List<UserProject?> { userProject1, userProject2 },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        var fakeRepository = A.Fake<IRepository<UserProject>>();
        A.CallTo(() => fakeRepository.GetAllAsync(A<PaginationParams>._))
            .Returns(Task.FromResult(paginatedResult));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<UserProject>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.UserProject.ListUserProjects.Endpoint>(fakeUnitOfWork);
        
        // Initialize response manually due to required property
        ep.Response = new WebApi.Endpoints.UserProject.ListUserProjects.Response 
        { 
            Result = new PaginatedResult<WebApi.Common.Dtos.UserProjectDto?> 
            { 
                Data = new List<WebApi.Common.Dtos.UserProjectDto?>(), 
                TotalCount = 0, 
                PageNumber = 1, 
                PageSize = 10 
            } 
        };
        
        var req = new WebApi.Endpoints.UserProject.ListUserProjects.Request
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
