namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa
{
    [DataContract]
    public class RemoveTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
