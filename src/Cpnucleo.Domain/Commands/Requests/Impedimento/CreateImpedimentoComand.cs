using Cpnucleo.Domain.Commands.Responses.Impedimento;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Impedimento
{
    public class CreateImpedimentoComand : IRequest<CreateImpedimentoResponse>
    {
        public Domain.Entities.Impedimento Impedimento { get; set; }
    }
}
