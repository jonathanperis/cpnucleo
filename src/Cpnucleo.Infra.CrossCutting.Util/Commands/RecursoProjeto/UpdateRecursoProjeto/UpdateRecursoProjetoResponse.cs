using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoProjeto
{
    [DataContract]
    public class UpdateRecursoProjetoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
