namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso
{
    [DataContract]
    public class ListRecursoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public IEnumerable<RecursoViewModel> Recursos { get; set; }
    }
}
