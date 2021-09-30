namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso
{
    [DataContract]
    public class UpdateRecursoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
