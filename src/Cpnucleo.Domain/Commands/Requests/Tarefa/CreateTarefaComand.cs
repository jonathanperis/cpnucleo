using Cpnucleo.Domain.Commands.Responses.Tarefa;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Tarefa
{
    public class CreateTarefaComand : IRequest<CreateTarefaResponse>
    {
        public Domain.Entities.Tarefa Tarefa { get; set; }
    }
}
