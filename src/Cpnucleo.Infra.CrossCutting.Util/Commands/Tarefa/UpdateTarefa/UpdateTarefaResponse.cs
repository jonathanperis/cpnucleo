namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa
{
    [DataContract]
    public class UpdateTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
