using Cpnucleo.Domain.Commands.Responses.Impedimento;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Impedimento
{
    public class UpdateImpedimentoComand : IRequest<UpdateImpedimentoResponse>
    {
        public Domain.Entities.Impedimento Impedimento { get; set; }
    }
}
