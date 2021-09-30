namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.CreateRecurso
{
    [DataContract]
    public class CreateRecursoCommand
    {
        [DataMember(Order = 1)]
        public RecursoViewModel Recurso { get; set; }
    }
}
