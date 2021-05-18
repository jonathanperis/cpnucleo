using Cpnucleo.Domain.Commands.Responses.RecursoTarefa;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.RecursoTarefa
{
    public class CreateRecursoTarefaComand : IRequest<CreateRecursoTarefaResponse>
    {
        public Domain.Entities.RecursoTarefa RecursoTarefa { get; set; }
    }
}
