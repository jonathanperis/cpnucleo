using AutoMapper;
using Cpnucleo.Application.Handlers;
using Cpnucleo.Application.Test.Helpers;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Application.Test.Handlers
{
    public class WorkflowHandlerTest
    {
        [Fact]
        public async Task CreateWorkflowCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            CreateWorkflowCommand request = new()
            {
                Workflow = MockViewModelHelper.GetNewWorkflow()
            };

            // Act
            WorkflowHandler handler = new(unitOfWork, mapper);
            CreateWorkflowResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Workflow != null);
            Assert.True(response.Workflow.Id != Guid.Empty);
            Assert.True(response.Workflow.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task GetWorkflowQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid workflowId = Guid.NewGuid();

            await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));
            await unitOfWork.SaveChangesAsync();

            GetWorkflowQuery request = new()
            {
                Id = workflowId
            };

            // Act
            WorkflowHandler handler = new(unitOfWork, mapper);
            GetWorkflowResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Workflow != null);
            Assert.True(response.Workflow.Id != Guid.Empty);
            Assert.True(response.Workflow.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task ListWorkflowQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid workflowId = Guid.NewGuid();

            await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));
            await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow());
            await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow());

            await unitOfWork.SaveChangesAsync();

            ListWorkflowQuery request = new();

            // Act
            WorkflowHandler handler = new(unitOfWork, mapper);
            ListWorkflowResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Workflows != null);
            Assert.True(response.Workflows.Any());
            Assert.True(response.Workflows.FirstOrDefault(x => x.Id == workflowId) != null);
            Assert.True(response.Workflows.FirstOrDefault(x => x.TamanhoColuna != null) != null);
        }

        [Fact]
        public async Task RemoveWorkflowCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid workflowId = Guid.NewGuid();

            Workflow workflow = MockEntityHelper.GetNewWorkflow(workflowId);

            await unitOfWork.WorkflowRepository.AddAsync(workflow);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.WorkflowRepository.Detatch(workflow);

            RemoveWorkflowCommand request = new()
            {
                Id = workflowId
            };

            GetWorkflowQuery request2 = new()
            {
                Id = workflowId
            };

            // Act
            WorkflowHandler handler = new(unitOfWork, mapper);
            RemoveWorkflowResponse response = await handler.Handle(request, CancellationToken.None);
            GetWorkflowResponse response2 = await handler.Handle(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.NotFound);
            Assert.True(response2.Workflow == null);
        }

        [Fact]
        public async Task UpdateWorkflowCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid workflowId = Guid.NewGuid();
            DateTime dataInclusao = DateTime.Now;

            Workflow workflow = MockEntityHelper.GetNewWorkflow(workflowId);

            await unitOfWork.WorkflowRepository.AddAsync(workflow);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.WorkflowRepository.Detatch(workflow);

            UpdateWorkflowCommand request = new()
            {
                Workflow = MockViewModelHelper.GetNewWorkflow(workflowId, dataInclusao)
            };

            GetWorkflowQuery request2 = new()
            {
                Id = workflowId
            };

            // Act
            WorkflowHandler handler = new(unitOfWork, mapper);
            UpdateWorkflowResponse response = await handler.Handle(request, CancellationToken.None);
            GetWorkflowResponse response2 = await handler.Handle(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.Success);
            Assert.True(response2.Workflow != null);
            Assert.True(response2.Workflow.Id == workflowId);
            Assert.True(response2.Workflow.DataInclusao.Ticks == dataInclusao.Ticks);
        }
    }
}
