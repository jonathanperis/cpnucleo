namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa
{
    [DataContract]
    public class RemoveRecursoTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
