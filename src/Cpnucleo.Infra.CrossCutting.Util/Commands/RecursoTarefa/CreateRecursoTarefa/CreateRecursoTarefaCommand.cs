namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa
{
    [DataContract]
    public class CreateRecursoTarefaCommand
    {
        [DataMember(Order = 1)]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }
    }
}
