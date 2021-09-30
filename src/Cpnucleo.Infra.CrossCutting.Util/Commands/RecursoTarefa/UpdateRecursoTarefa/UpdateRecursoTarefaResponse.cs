namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa
{
    [DataContract]
    public class UpdateRecursoTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
