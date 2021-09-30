namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.ListRecursoProjeto;

[DataContract]
public class ListRecursoProjetoResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public IEnumerable<RecursoProjetoViewModel> RecursoProjetos { get; set; }
}