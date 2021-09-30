namespace Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.GetTipoTarefa
{
    [DataContract]
    public class GetTipoTarefaQuery
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
