using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.GetTipoTarefa
{
    [DataContract]
    public class GetTipoTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public TipoTarefaViewModel TipoTarefa { get; set; }
    }
}
