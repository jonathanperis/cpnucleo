namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.RemoveRecurso
{
    [DataContract]
    public class RemoveRecursoCommand
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
