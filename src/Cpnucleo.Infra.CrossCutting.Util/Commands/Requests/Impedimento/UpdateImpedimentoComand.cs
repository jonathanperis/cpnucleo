using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Impedimento
{
    public class UpdateImpedimentoComand : IRequest<UpdateImpedimentoResponse>
    {
        public ImpedimentoViewModel Impedimento { get; set; }
    }
}
