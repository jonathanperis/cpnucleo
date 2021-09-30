namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;

[DataContract]
public class ListRecursoQuery
{
    [DataMember(Order = 1)]
    public bool GetDependencies { get; set; }
}