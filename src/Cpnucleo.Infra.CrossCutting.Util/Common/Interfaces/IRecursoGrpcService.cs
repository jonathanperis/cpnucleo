namespace Cpnucleo.Infra.CrossCutting.Util.Common.Interfaces;

public interface IRecursoGrpcService : IService<IRecursoGrpcService>
{
    UnaryResult<OperationResult> CreateRecurso(CreateRecursoCommand command);

    UnaryResult<OperationResult> UpdateRecurso(UpdateRecursoCommand command);

    UnaryResult<GetRecursoViewModel> GetRecurso(GetRecursoQuery query);

    UnaryResult<ListRecursoViewModel> ListRecurso(ListRecursoQuery query);

    UnaryResult<OperationResult> RemoveRecurso(RemoveRecursoCommand command);
}