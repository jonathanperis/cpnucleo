namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa
{
    [DataContract]
    public class ListTipoTarefaQuery
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
