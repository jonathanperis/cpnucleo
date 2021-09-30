namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;

[DataContract]
public class GetProjetoQuery
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}