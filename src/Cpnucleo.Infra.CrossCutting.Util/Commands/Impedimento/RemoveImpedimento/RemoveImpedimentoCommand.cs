using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento
{
    [DataContract]
    public class RemoveImpedimentoCommand
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
