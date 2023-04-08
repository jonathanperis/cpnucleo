using Cpnucleo.Shared.Commands.CreateApontamento;
using Cpnucleo.Shared.Commands.RemoveApontamento;
using Cpnucleo.Shared.Commands.UpdateApontamento;
using Cpnucleo.Shared.Queries.GetApontamento;
using Cpnucleo.Shared.Queries.ListApontamento;
using Cpnucleo.Shared.Queries.ListApontamentoByRecurso;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IApontamentoGrpcService : IService<IApontamentoGrpcService>
{
    UnaryResult<OperationResult> CreateApontamento(CreateApontamentoCommand command);

    UnaryResult<OperationResult> UpdateApontamento(UpdateApontamentoCommand command);

    UnaryResult<GetApontamentoViewModel> GetApontamento(GetApontamentoQuery query);

    UnaryResult<ListApontamentoViewModel> ListApontamento(ListApontamentoQuery query);

    UnaryResult<OperationResult> RemoveApontamento(RemoveApontamentoCommand command);

    UnaryResult<ListApontamentoByRecursoViewModel> GetApontamentoByRecurso(ListApontamentoByRecursoQuery query);
}