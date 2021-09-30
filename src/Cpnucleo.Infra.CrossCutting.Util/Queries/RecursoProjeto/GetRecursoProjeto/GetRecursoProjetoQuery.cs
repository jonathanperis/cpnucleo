namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetRecursoProjeto;

[DataContract]
public class GetRecursoProjetoQuery
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}