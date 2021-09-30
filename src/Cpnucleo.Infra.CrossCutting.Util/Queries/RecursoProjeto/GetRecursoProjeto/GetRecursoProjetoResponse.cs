namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetRecursoProjeto;

[DataContract]
public class GetRecursoProjetoResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public RecursoProjetoViewModel RecursoProjeto { get; set; }
}