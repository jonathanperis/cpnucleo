using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface IRecursoProjetoGrpcService : IService<IRecursoProjetoGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateRecursoProjetoCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateRecursoProjetoCommand command);

    UnaryResult<RecursoProjetoViewModel> GetAsync(GetRecursoProjetoQuery query);

    UnaryResult<IEnumerable<RecursoProjetoViewModel>> AllAsync(ListRecursoProjetoQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveRecursoProjetoCommand command);

    UnaryResult<IEnumerable<RecursoProjetoViewModel>> GetByProjetoAsync(GetByProjetoQuery query);
}