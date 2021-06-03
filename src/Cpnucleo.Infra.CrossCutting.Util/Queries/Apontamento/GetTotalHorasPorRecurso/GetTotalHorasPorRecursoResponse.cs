using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetTotalHorasPorRecurso
{
    [DataContract]
    public class GetTotalHorasPorRecursoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public int TotalHoras { get; set; }
    }
}
