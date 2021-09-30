namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa;

[DataContract]
public class RemoveTarefaCommand
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}