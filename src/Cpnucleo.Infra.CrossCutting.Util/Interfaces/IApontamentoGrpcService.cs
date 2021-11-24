using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface IApontamentoGrpcService : IService<IApontamentoGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateApontamentoCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateApontamentoCommand command);

    UnaryResult<ApontamentoViewModel> GetAsync(GetApontamentoQuery query);

    UnaryResult<IEnumerable<ApontamentoViewModel>> AllAsync(ListApontamentoQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveApontamentoCommand command);

    UnaryResult<IEnumerable<ApontamentoViewModel>> GetByRecursoAsync(GetByRecursoQuery query);
}