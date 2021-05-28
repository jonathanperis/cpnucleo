using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Recurso
{
    [DataContract]
    public class UpdateRecursoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
