using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema
{
    [DataContract]
    public class RemoveSistemaCommand
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
