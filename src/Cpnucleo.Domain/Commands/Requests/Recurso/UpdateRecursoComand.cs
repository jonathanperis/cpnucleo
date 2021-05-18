using Cpnucleo.Domain.Commands.Responses.Recurso;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Recurso
{
    public class UpdateRecursoComand : IRequest<UpdateRecursoResponse>
    {
        public Domain.Entities.Recurso Recurso { get; set; }
    }
}
