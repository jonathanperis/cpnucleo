namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateByWorkflow;

[DataContract]
public class UpdateByWorkflowResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }
}