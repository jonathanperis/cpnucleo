namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.RemoveRecursoProjeto;

[DataContract]
public class RemoveRecursoProjetoResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }
}