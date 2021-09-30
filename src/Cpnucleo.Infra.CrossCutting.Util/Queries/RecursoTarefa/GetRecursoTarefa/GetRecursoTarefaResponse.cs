namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa
{
    [DataContract]
    public class GetRecursoTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }
    }
}
