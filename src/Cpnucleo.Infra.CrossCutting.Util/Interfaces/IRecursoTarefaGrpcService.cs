using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface IRecursoTarefaGrpcService : IService<IRecursoTarefaGrpcService>
{
    UnaryResult<CreateRecursoTarefaResponse> AddAsync(CreateRecursoTarefaCommand command);

    UnaryResult<UpdateRecursoTarefaResponse> UpdateAsync(UpdateRecursoTarefaCommand command);

    UnaryResult<GetRecursoTarefaResponse> GetAsync(GetRecursoTarefaQuery query);

    UnaryResult<ListRecursoTarefaResponse> AllAsync(ListRecursoTarefaQuery query);

    UnaryResult<RemoveRecursoTarefaResponse> RemoveAsync(RemoveRecursoTarefaCommand command);

    UnaryResult<GetByTarefaResponse> GetByTarefaAsync(GetByTarefaQuery query);
}