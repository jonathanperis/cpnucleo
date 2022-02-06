using Cpnucleo.Application.Commands.Recurso.CreateRecurso;
using Cpnucleo.Application.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Application.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Application.Queries.Recurso.GetRecurso;
using Cpnucleo.Application.Queries.Recurso.ListRecurso;

namespace Cpnucleo.Application.Interfaces;

public interface IRecursoGrpcService : IService<IRecursoGrpcService>
{
    UnaryResult<OperationResult> CreateRecurso(CreateRecursoCommand command);

    UnaryResult<OperationResult> UpdateRecurso(UpdateRecursoCommand command);

    UnaryResult<GetRecursoViewModel> GetRecurso(GetRecursoQuery query);

    UnaryResult<ListRecursoViewModel> ListRecurso(ListRecursoQuery query);

    UnaryResult<OperationResult> RemoveRecurso(RemoveRecursoCommand command);
}