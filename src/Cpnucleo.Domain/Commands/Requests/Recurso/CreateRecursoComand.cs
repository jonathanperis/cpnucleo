using Cpnucleo.Domain.Commands.Responses.Recurso;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Recurso
{
    public class CreateRecursoComand : IRequest<CreateRecursoResponse>
    {
        public Domain.Entities.Recurso Recurso { get; set; }
    }
}
