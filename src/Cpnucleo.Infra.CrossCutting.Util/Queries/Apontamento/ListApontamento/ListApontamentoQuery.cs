namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;

[DataContract]
public class ListApontamentoQuery
{
    [DataMember(Order = 1)]
    public bool GetDependencies { get; set; }
}