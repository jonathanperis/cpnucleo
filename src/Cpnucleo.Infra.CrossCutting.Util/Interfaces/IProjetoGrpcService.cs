using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface IProjetoGrpcService : IService<IProjetoGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateProjetoCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateProjetoCommand command);

    UnaryResult<ProjetoViewModel> GetAsync(GetProjetoQuery query);

    UnaryResult<IEnumerable<ProjetoViewModel>> AllAsync(ListProjetoQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveProjetoCommand command);
}