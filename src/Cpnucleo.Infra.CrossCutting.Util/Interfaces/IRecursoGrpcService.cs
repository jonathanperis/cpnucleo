namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.CreateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;

public interface IRecursoGrpcService : IService<IRecursoGrpcService>
{
    UnaryResult<CreateRecursoResponse> AddAsync(CreateRecursoCommand command);

    UnaryResult<UpdateRecursoResponse> UpdateAsync(UpdateRecursoCommand command);

    UnaryResult<GetRecursoResponse> GetAsync(GetRecursoQuery query);

    UnaryResult<ListRecursoResponse> AllAsync(ListRecursoQuery query);

    UnaryResult<RemoveRecursoResponse> RemoveAsync(RemoveRecursoCommand command);

    UnaryResult<AuthResponse> AuthAsync(AuthQuery query);
}