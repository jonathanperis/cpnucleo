namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.RemoveRecurso;

[DataContract]
public class RemoveRecursoResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }
}