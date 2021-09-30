namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa
{
    [DataContract]
    public class ListRecursoTarefaQuery
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
