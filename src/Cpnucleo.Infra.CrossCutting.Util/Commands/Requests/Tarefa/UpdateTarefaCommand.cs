using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Tarefa
{
    public class UpdateTarefaCommand : IRequest<UpdateTarefaResponse>
    {
        public TarefaViewModel Tarefa { get; set; }
    }
}
