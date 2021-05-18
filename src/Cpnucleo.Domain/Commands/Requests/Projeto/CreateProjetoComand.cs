using Cpnucleo.Domain.Commands.Responses.Projeto;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Projeto
{
    public class CreateProjetoComand : IRequest<CreateProjetoResponse>
    {
        public Domain.Entities.Projeto Projeto { get; set; }
    }
}
