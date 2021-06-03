using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto
{
    [DataContract]
    public class GetRecursoProjetoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }
    }
}
