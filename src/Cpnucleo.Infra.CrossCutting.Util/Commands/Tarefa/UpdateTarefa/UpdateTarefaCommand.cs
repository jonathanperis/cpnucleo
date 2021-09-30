namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa
{
    [DataContract]
    public class UpdateTarefaCommand
    {
        [DataMember(Order = 1)]
        public TarefaViewModel Tarefa { get; set; }
    }
}
