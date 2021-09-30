namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa
{
    [DataContract]
    public class ListTarefaQuery
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
