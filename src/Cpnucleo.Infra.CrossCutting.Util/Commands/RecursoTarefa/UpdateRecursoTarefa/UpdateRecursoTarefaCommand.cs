namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa;

[DataContract]
public class UpdateRecursoTarefaCommand
{
    [DataMember(Order = 1)]
    public RecursoTarefaViewModel RecursoTarefa { get; set; }
}