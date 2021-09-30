namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.ListRecursoProjeto;

[DataContract]
public class ListRecursoProjetoQuery
{
    [DataMember(Order = 1)]
    public bool GetDependencies { get; set; }
}