namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento
{
    [DataContract]
    public class GetApontamentoQuery
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
