using Cpnucleo.Domain.Commands.Responses.RecursoProjeto;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.RecursoProjeto
{
    public class UpdateRecursoProjetoComand : IRequest<UpdateRecursoProjetoResponse>
    {
        public Domain.Entities.RecursoProjeto RecursoProjeto { get; set; }
    }
}
