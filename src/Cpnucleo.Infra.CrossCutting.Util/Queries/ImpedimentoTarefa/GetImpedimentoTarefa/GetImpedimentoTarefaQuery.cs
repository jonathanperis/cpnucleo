using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetImpedimentoTarefa
{
    [DataContract]
    public class GetImpedimentoTarefaQuery
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
