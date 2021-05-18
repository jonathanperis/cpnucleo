using Cpnucleo.Domain.Commands.Responses.Apontamento;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Apontamento
{
    public class CreateApontamentoComand : IRequest<CreateApontamentoResponse>
    {
        public Domain.Entities.Apontamento Apontamento { get; set; }
    }
}
