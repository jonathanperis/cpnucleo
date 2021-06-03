using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.ImpedimentoTarefa
{
    [DataContract]
    public class CreateImpedimentoTarefaCommand : IRequest<CreateImpedimentoTarefaResponse>
    {
        [DataMember(Order = 1)]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
    }
}
