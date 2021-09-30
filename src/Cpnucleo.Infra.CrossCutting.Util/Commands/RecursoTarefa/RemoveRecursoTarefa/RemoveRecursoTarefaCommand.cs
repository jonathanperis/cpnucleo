namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa;

[DataContract]
public class RemoveRecursoTarefaCommand
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}