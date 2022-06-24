using Cpnucleo.Shared.Commands.Apontamento;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Apontamento;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IApontamentoGrpcService : IService<IApontamentoGrpcService>
{
    UnaryResult<OperationResult> CreateApontamento(CreateApontamentoCommand command);

    UnaryResult<OperationResult> UpdateApontamento(UpdateApontamentoCommand command);

    UnaryResult<GetApontamentoViewModel> GetApontamento(GetApontamentoQuery query);

    UnaryResult<ListApontamentoViewModel> ListApontamento(ListApontamentoQuery query);

    UnaryResult<OperationResult> RemoveApontamento(RemoveApontamentoCommand command);

    UnaryResult<GetApontamentoByRecursoViewModel> GetApontamentoByRecurso(GetApontamentoByRecursoQuery query);
}