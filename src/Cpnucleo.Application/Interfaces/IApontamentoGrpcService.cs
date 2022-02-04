using Cpnucleo.Application.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Application.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Application.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Application.Queries.Apontamento.GetApontamento;
using Cpnucleo.Application.Queries.Apontamento.GetByRecurso;
using Cpnucleo.Application.Queries.Apontamento.ListApontamento;

namespace Cpnucleo.Application.Interfaces;

public interface IApontamentoGrpcService : IService<IApontamentoGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateApontamentoCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateApontamentoCommand command);

    UnaryResult<GetApontamentoViewModel> GetAsync(GetApontamentoQuery query);

    UnaryResult<ListApontamentoViewModel> AllAsync(ListApontamentoQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveApontamentoCommand command);

    UnaryResult<GetByRecursoViewModel> GetByRecursoAsync(GetByRecursoQuery query);
}