namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso
{
    [DataContract]
    public class UpdateRecursoCommand
    {
        [DataMember(Order = 1)]
        public RecursoViewModel Recurso { get; set; }
    }
}
