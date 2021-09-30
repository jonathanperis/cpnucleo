namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa
{
    [DataContract]
    public class ListTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public IEnumerable<TarefaViewModel> Tarefas { get; set; }
    }
}
