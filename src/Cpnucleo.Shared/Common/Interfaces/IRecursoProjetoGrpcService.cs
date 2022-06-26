namespace Cpnucleo.Shared.Common.Interfaces;

public interface IRecursoProjetoGrpcService : IService<IRecursoProjetoGrpcService>
{
    UnaryResult<OperationResult> CreateRecursoProjeto(CreateRecursoProjetoCommand command);

    UnaryResult<OperationResult> UpdateRecursoProjeto(UpdateRecursoProjetoCommand command);

    UnaryResult<GetRecursoProjetoViewModel> GetRecursoProjeto(GetRecursoProjetoQuery query);

    UnaryResult<ListRecursoProjetoViewModel> ListRecursoProjeto(ListRecursoProjetoQuery query);

    UnaryResult<OperationResult> RemoveRecursoProjeto(RemoveRecursoProjetoCommand command);

    UnaryResult<GetRecursoProjetoByProjetoViewModel> GetRecursoProjetoByProjeto(GetRecursoProjetoByProjetoQuery query);
}