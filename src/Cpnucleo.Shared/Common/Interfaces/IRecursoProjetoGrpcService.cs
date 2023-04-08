using Cpnucleo.Shared.Commands.CreateRecursoProjeto;
using Cpnucleo.Shared.Commands.RemoveRecursoProjeto;
using Cpnucleo.Shared.Commands.UpdateRecursoProjeto;
using Cpnucleo.Shared.Queries.GetRecursoProjeto;
using Cpnucleo.Shared.Queries.ListRecursoProjeto;
using Cpnucleo.Shared.Queries.ListRecursoProjetoByProjeto;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IRecursoProjetoGrpcService : IService<IRecursoProjetoGrpcService>
{
    UnaryResult<OperationResult> CreateRecursoProjeto(CreateRecursoProjetoCommand command);

    UnaryResult<OperationResult> UpdateRecursoProjeto(UpdateRecursoProjetoCommand command);

    UnaryResult<GetRecursoProjetoViewModel> GetRecursoProjeto(GetRecursoProjetoQuery query);

    UnaryResult<ListRecursoProjetoViewModel> ListRecursoProjeto(ListRecursoProjetoQuery query);

    UnaryResult<OperationResult> RemoveRecursoProjeto(RemoveRecursoProjetoCommand command);

    UnaryResult<ListRecursoProjetoByProjetoViewModel> GetRecursoProjetoByProjeto(ListRecursoProjetoByProjetoQuery query);
}