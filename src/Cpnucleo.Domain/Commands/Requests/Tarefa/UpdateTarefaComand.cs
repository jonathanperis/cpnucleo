using Cpnucleo.Domain.Commands.Responses.Tarefa;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Tarefa
{
    public class UpdateTarefaComand : IRequest<UpdateTarefaResponse>
    {
        public Domain.Entities.Tarefa Tarefa { get; set; }
    }
}
