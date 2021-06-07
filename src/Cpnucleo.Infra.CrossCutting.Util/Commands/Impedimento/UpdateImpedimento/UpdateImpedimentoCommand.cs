using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento
{
    [DataContract]
    public class UpdateImpedimentoCommand
    {
        [DataMember(Order = 1)]
        public ImpedimentoViewModel Impedimento { get; set; }
    }
}
