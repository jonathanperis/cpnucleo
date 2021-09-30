namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;

[DataContract]
public class GetSistemaQuery
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}