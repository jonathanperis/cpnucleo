using Cpnucleo.Application.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Application.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Application.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Application.Queries.Apontamento.GetApontamento;
using Cpnucleo.Application.Queries.Apontamento.GetApontamentoByRecurso;
using Cpnucleo.Application.Queries.Apontamento.ListApontamento;

namespace Cpnucleo.Application.Interfaces;

public interface IApontamentoGrpcService : IService<IApontamentoGrpcService>
{
    UnaryResult<OperationResult> CreateApontamento(CreateApontamentoCommand command);

    UnaryResult<OperationResult> UpdateApontamento(UpdateApontamentoCommand command);

    UnaryResult<GetApontamentoViewModel> GetApontamento(GetApontamentoQuery query);

    UnaryResult<ListApontamentoViewModel> ListApontamento(ListApontamentoQuery query);

    UnaryResult<OperationResult> RemoveApontamento(RemoveApontamentoCommand command);

    UnaryResult<GetApontamentoByRecursoViewModel> GetApontamentoByRecurso(GetApontamentoByRecursoQuery query);
}