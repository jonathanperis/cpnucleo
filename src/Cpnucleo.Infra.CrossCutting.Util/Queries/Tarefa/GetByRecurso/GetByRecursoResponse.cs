namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetByRecurso
{
    [DataContract]
    public class GetByRecursoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public IEnumerable<TarefaViewModel> Tarefas { get; set; }
    }
}
