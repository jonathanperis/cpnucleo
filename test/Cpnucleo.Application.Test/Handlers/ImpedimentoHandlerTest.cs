using AutoMapper;
using Cpnucleo.Application.Handlers;
using Cpnucleo.Application.Test.Helpers;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Application.Test.Handlers
{
    public class ImpedimentoHandlerTest
    {
        [Fact]
        public async Task CreateImpedimentoCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            CreateImpedimentoCommand request = new()
            {
                Impedimento = MockViewModelHelper.GetNewImpedimento()
            };

            // Act
            ImpedimentoHandler handler = new(unitOfWork, mapper);
            CreateImpedimentoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Impedimento != null);
            Assert.True(response.Impedimento.Id != Guid.Empty);
            Assert.True(response.Impedimento.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task GetImpedimentoQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid impedimentoId = Guid.NewGuid();

            await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));
            await unitOfWork.SaveChangesAsync();

            GetImpedimentoQuery request = new()
            {
                Id = impedimentoId
            };

            // Act
            ImpedimentoHandler handler = new(unitOfWork, mapper);
            GetImpedimentoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Impedimento != null);
            Assert.True(response.Impedimento.Id != Guid.Empty);
            Assert.True(response.Impedimento.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task ListImpedimentoQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid impedimentoId = Guid.NewGuid();

            await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));
            await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento());
            await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento());

            await unitOfWork.SaveChangesAsync();

            ListImpedimentoQuery request = new();

            // Act
            ImpedimentoHandler handler = new(unitOfWork, mapper);
            ListImpedimentoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Impedimentos != null);
            Assert.True(response.Impedimentos.Any());
            Assert.True(response.Impedimentos.FirstOrDefault(x => x.Id == impedimentoId) != null);
        }

        [Fact]
        public async Task RemoveImpedimentoCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid impedimentoId = Guid.NewGuid();

            Impedimento impedimento = MockEntityHelper.GetNewImpedimento(impedimentoId);

            await unitOfWork.ImpedimentoRepository.AddAsync(impedimento);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.ImpedimentoRepository.Detatch(impedimento);

            RemoveImpedimentoCommand request = new()
            {
                Id = impedimentoId
            };

            GetImpedimentoQuery request2 = new()
            {
                Id = impedimentoId
            };

            // Act
            ImpedimentoHandler handler = new(unitOfWork, mapper);
            RemoveImpedimentoResponse response = await handler.InvokeAsync(request, CancellationToken.None);
            GetImpedimentoResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.NotFound);
            Assert.True(response2.Impedimento == null);
        }

        [Fact]
        public async Task UpdateImpedimentoCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid impedimentoId = Guid.NewGuid();
            DateTime dataInclusao = DateTime.Now;

            Impedimento impedimento = MockEntityHelper.GetNewImpedimento(impedimentoId);

            await unitOfWork.ImpedimentoRepository.AddAsync(impedimento);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.ImpedimentoRepository.Detatch(impedimento);

            UpdateImpedimentoCommand request = new()
            {
                Impedimento = MockViewModelHelper.GetNewImpedimento(impedimentoId, dataInclusao)
            };

            GetImpedimentoQuery request2 = new()
            {
                Id = impedimentoId
            };

            // Act
            ImpedimentoHandler handler = new(unitOfWork, mapper);
            UpdateImpedimentoResponse response = await handler.InvokeAsync(request, CancellationToken.None);
            GetImpedimentoResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.Success);
            Assert.True(response2.Impedimento != null);
            Assert.True(response2.Impedimento.Id == impedimentoId);
            Assert.True(response2.Impedimento.DataInclusao.Ticks == dataInclusao.Ticks);
        }
    }
}
