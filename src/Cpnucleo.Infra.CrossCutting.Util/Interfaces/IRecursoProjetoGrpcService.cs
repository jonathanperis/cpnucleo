using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetByProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.ListRecursoProjeto;
using MagicOnion;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    public interface IRecursoProjetoGrpcService : IService<IRecursoProjetoGrpcService>
    {
        UnaryResult<CreateRecursoProjetoResponse> AddAsync(CreateRecursoProjetoCommand command);

        UnaryResult<UpdateRecursoProjetoResponse> UpdateAsync(UpdateRecursoProjetoCommand command);

        UnaryResult<GetRecursoProjetoResponse> GetAsync(GetRecursoProjetoQuery query);

        UnaryResult<ListRecursoProjetoResponse> AllAsync(ListRecursoProjetoQuery query);

        UnaryResult<RemoveRecursoProjetoResponse> RemoveAsync(RemoveRecursoProjetoCommand command);

        UnaryResult<GetByProjetoResponse> GetByProjetoAsync(GetByProjetoQuery query);
    }
}
