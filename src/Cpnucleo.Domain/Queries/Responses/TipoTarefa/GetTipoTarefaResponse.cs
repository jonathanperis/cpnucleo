namespace Cpnucleo.Domain.Queries.Responses.TipoTarefa
{
    public class GetTipoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.TipoTarefa TipoTarefa { get; set; }
    }
}
