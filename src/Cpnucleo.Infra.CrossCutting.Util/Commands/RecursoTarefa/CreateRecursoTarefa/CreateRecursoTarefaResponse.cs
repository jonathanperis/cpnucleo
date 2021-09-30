namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;

[DataContract]
public class CreateRecursoTarefaResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public RecursoTarefaViewModel RecursoTarefa { get; set; }
}