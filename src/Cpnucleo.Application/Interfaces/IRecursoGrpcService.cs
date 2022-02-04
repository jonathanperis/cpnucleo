using Cpnucleo.Application.Commands.Recurso.CreateRecurso;
using Cpnucleo.Application.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Application.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Application.Queries.Recurso.GetRecurso;
using Cpnucleo.Application.Queries.Recurso.ListRecurso;

namespace Cpnucleo.Application.Interfaces;

public interface IRecursoGrpcService : IService<IRecursoGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateRecursoCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateRecursoCommand command);

    UnaryResult<GetRecursoViewModel> GetAsync(GetRecursoQuery query);

    UnaryResult<ListRecursoViewModel> AllAsync(ListRecursoQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveRecursoCommand command);
}