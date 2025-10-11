using FakeItEasy;
using FastEndpoints;
using Shouldly;
using Domain.UoW;
using Domain.Repositories;
using Domain.Models;
using Domain.Entities;
using Infrastructure.Common.Context;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Unit.Tests.Endpoints;

[TestFixture]
public class WorkflowEndpointsTests
{
    [Test]
    public async Task GetWorkflowById_WithValidId_ShouldReturnWorkflow()
    {
        // Arrange
        var workflowId = Guid.NewGuid();
        var workflow = Workflow.Create("Test Workflow", 1, workflowId);
        
        var fakeRepository = A.Fake<IRepository<Workflow>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(workflowId))
            .Returns(Task.FromResult<Workflow?>(workflow));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Workflow>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Workflow.GetWorkflowById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Workflow.GetWorkflowById.Request { Id = workflowId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Workflow.ShouldNotBeNull();
        ep.Response.Workflow.Id.ShouldBe(workflowId);
        ep.Response.Workflow.Name.ShouldBe("Test Workflow");
    }

    [Test]
    public async Task GetWorkflowById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var workflowId = Guid.NewGuid();
        
        var fakeRepository = A.Fake<IRepository<Workflow>>();
        A.CallTo(() => fakeRepository.GetByIdAsync(workflowId))
            .Returns(Task.FromResult<Workflow?>(null));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Workflow>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Workflow.GetWorkflowById.Endpoint>(fakeUnitOfWork);
        var req = new WebApi.Endpoints.Workflow.GetWorkflowById.Request { Id = workflowId };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.HttpContext.Response.StatusCode.ShouldBe(404);
    }

    [Test]
    [Ignore("EF Core DbSet.Any() extension method cannot be mocked with FakeItEasy. Use integration tests for EF Core-based endpoints.")]
    public async Task CreateWorkflow_WithValidData_ShouldCreateWorkflow()
    {
        // Arrange
        var workflowId = Guid.NewGuid();
        var workflow = Workflow.Create("New Workflow", 1, workflowId);
        
        var fakeDbContext = A.Fake<IApplicationDbContext>();
        var fakeDbSet = A.Fake<DbSet<Workflow>>();
        
        A.CallTo(() => fakeDbContext.Workflows).Returns(fakeDbSet);
        A.CallTo(() => fakeDbSet.Any(A<System.Linq.Expressions.Expression<Func<Workflow, bool>>>._)).Returns(false);
        A.CallTo(() => fakeDbContext.SaveChangesAsync(A<CancellationToken>._)).Returns(true);
        A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._, A<CancellationToken>._)).Returns(new ValueTask<Workflow?>(workflow));

        var ep = Factory.Create<WebApi.Endpoints.Workflow.CreateWorkflow.Endpoint>(fakeDbContext);
        var req = new WebApi.Endpoints.Workflow.CreateWorkflow.Request
        {
            Id = workflowId,
            Name = "New Workflow",
            Order = 1
        };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.ValidationFailed.ShouldBeFalse();
        ep.Response.ShouldNotBeNull();
        ep.Response.Workflow.ShouldNotBeNull();
        ep.Response.Workflow.Name.ShouldBe("New Workflow");
    }

    [Test]
    public async Task UpdateWorkflow_WithValidData_ShouldUpdateWorkflow()
    {
        // Arrange
        var workflowId = Guid.NewGuid();
        var workflow = Workflow.Create("Original Workflow", 1, workflowId);
        
        var fakeDbContext = A.Fake<IApplicationDbContext>();
        var fakeDbSet = A.Fake<DbSet<Workflow>>();
        
        A.CallTo(() => fakeDbContext.Workflows).Returns(fakeDbSet);
        A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._, A<CancellationToken>._)).Returns(new ValueTask<Workflow?>(workflow));
        A.CallTo(() => fakeDbContext.SaveChangesAsync(A<CancellationToken>._)).Returns(true);

        var ep = Factory.Create<WebApi.Endpoints.Workflow.UpdateWorkflow.Endpoint>(fakeDbContext);
        var req = new WebApi.Endpoints.Workflow.UpdateWorkflow.Request
        {
            Id = workflowId,
            Name = "Updated Workflow",
            Order = 2
        };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Success.ShouldBeTrue();
    }

    [Test]
    public async Task RemoveWorkflow_WithValidId_ShouldDeleteWorkflow()
    {
        // Arrange
        var workflowId = Guid.NewGuid();
        var workflow = Workflow.Create("Workflow to Delete", 1, workflowId);
        
        var fakeDbContext = A.Fake<IApplicationDbContext>();
        var fakeDbSet = A.Fake<DbSet<Workflow>>();
        
        A.CallTo(() => fakeDbContext.Workflows).Returns(fakeDbSet);
        A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._, A<CancellationToken>._)).Returns(new ValueTask<Workflow?>(workflow));
        A.CallTo(() => fakeDbContext.SaveChangesAsync(A<CancellationToken>._)).Returns(true);

        var ep = Factory.Create<WebApi.Endpoints.Workflow.RemoveWorkflow.Endpoint>(fakeDbContext);
        var req = new WebApi.Endpoints.Workflow.RemoveWorkflow.Request { Ids = new List<Guid> { workflowId } };

        // Act
        await ep.HandleAsync(req, default);

        // Assert
        ep.Response.ShouldNotBeNull();
        ep.Response.Success.ShouldBeTrue();
    }

    [Test]
    public async Task ListWorkflows_ShouldReturnPaginatedResults()
    {
        // Arrange
        var workflowId1 = Guid.NewGuid();
        var workflowId2 = Guid.NewGuid();
        var workflow1 = Workflow.Create("Workflow 1", 1, workflowId1);
        var workflow2 = Workflow.Create("Workflow 2", 2, workflowId2);
        
        var paginatedResult = new PaginatedResult<Workflow?>
        {
            Data = new List<Workflow?> { workflow1, workflow2 },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        var fakeRepository = A.Fake<IRepository<Workflow>>();
        A.CallTo(() => fakeRepository.GetAllAsync(A<PaginationParams>._))
            .Returns(Task.FromResult(paginatedResult));

        var fakeUnitOfWork = A.Fake<IUnitOfWork>();
        A.CallTo(() => fakeUnitOfWork.GetRepository<Workflow>()).Returns(fakeRepository);

        var ep = Factory.Create<WebApi.Endpoints.Workflow.ListWorkflows.Endpoint>(fakeUnitOfWork);
        
        // Initialize response manually due to required property
        ep.Response = new WebApi.Endpoints.Workflow.ListWorkflows.Response 
        { 
            Result = new PaginatedResult<WebApi.Common.Dtos.WorkflowDto?> 
            { 
                Data = new List<WebApi.Common.Dtos.WorkflowDto?>(), 
                TotalCount = 0, 
                PageNumber = 1, 
                PageSize = 10 
            } 
        };
        
        var req = new WebApi.Endpoints.Workflow.ListWorkflows.Request
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
