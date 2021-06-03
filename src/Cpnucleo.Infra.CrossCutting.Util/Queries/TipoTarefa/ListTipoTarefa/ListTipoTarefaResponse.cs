using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa
{
    [DataContract]
    public class ListTipoTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public IEnumerable<TipoTarefaViewModel> TipoTarefas { get; set; }
    }
}
