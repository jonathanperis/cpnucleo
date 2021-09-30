namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;

public interface ITarefaGrpcService : IService<ITarefaGrpcService>
{
    UnaryResult<CreateTarefaResponse> AddAsync(CreateTarefaCommand command);

    UnaryResult<UpdateTarefaResponse> UpdateAsync(UpdateTarefaCommand command);

    UnaryResult<GetTarefaResponse> GetAsync(GetTarefaQuery query);

    UnaryResult<ListTarefaResponse> AllAsync(ListTarefaQuery query);

    UnaryResult<RemoveTarefaResponse> RemoveAsync(RemoveTarefaCommand command);

    UnaryResult<GetByRecursoResponse> GetByRecursoAsync(GetByRecursoQuery query);
}