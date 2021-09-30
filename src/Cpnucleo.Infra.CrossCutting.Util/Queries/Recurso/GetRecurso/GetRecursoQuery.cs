namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso;

[DataContract]
public class GetRecursoQuery
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}