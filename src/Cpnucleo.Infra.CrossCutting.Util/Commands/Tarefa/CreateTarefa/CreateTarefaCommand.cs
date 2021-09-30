namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;

[DataContract]
public class CreateTarefaCommand
{
    [DataMember(Order = 1)]
    public TarefaViewModel Tarefa { get; set; }
}