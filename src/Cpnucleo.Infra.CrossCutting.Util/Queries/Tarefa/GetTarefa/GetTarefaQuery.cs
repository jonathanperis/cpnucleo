namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa
{
    [DataContract]
    public class GetTarefaQuery
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
