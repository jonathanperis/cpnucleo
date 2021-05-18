using Cpnucleo.Domain.Commands.Responses.Projeto;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Projeto
{
    public class UpdateProjetoComand : IRequest<UpdateProjetoResponse>
    {
        public Domain.Entities.Projeto Projeto { get; set; }
    }
}
