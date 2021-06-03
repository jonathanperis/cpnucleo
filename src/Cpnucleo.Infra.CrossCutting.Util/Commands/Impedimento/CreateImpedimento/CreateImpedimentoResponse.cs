using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento
{
    [DataContract]
    public class CreateImpedimentoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public ImpedimentoViewModel Impedimento { get; set; }
    }
}
