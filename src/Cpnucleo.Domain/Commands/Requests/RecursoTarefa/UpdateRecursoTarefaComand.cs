using Cpnucleo.Domain.Commands.Responses.RecursoTarefa;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.RecursoTarefa
{
    public class UpdateRecursoTarefaComand : IRequest<UpdateRecursoTarefaResponse>
    {
        public Domain.Entities.RecursoTarefa RecursoTarefa { get; set; }
    }
}
