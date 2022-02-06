using Cpnucleo.Application.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Application.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Application.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.GetRecursoProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.GetRecursoProjetoByProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.ListRecursoProjeto;
using MagicOnion;

namespace Cpnucleo.Application.Interfaces;

public interface IRecursoProjetoGrpcService : IService<IRecursoProjetoGrpcService>
{
    UnaryResult<OperationResult> CreateRecursoProjeto(CreateRecursoProjetoCommand command);

    UnaryResult<OperationResult> UpdateRecursoProjeto(UpdateRecursoProjetoCommand command);

    UnaryResult<GetRecursoProjetoViewModel> GetRecursoProjeto(GetRecursoProjetoQuery query);

    UnaryResult<ListRecursoProjetoViewModel> ListRecursoProjeto(ListRecursoProjetoQuery query);

    UnaryResult<OperationResult> RemoveRecursoProjeto(RemoveRecursoProjetoCommand command);

    UnaryResult<GetRecursoProjetoByProjetoViewModel> GetRecursoProjetoByProjeto(GetRecursoProjetoByProjetoQuery query);
}