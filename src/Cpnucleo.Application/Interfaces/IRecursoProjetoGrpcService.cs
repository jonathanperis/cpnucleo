using Cpnucleo.Application.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Application.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Application.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.GetByProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.GetRecursoProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.ListRecursoProjeto;

namespace Cpnucleo.Application.Interfaces;

public interface IRecursoProjetoGrpcService : IService<IRecursoProjetoGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateRecursoProjetoCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateRecursoProjetoCommand command);

    UnaryResult<GetRecursoProjetoViewModel> GetAsync(GetRecursoProjetoQuery query);

    UnaryResult<ListRecursoProjetoViewModel> AllAsync(ListRecursoProjetoQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveRecursoProjetoCommand command);

    UnaryResult<GetRecursoProjetoByProjetoViewModel> GetByProjetoAsync(GetRecursoProjetoByProjetoQuery query);
}