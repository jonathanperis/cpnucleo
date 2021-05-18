namespace Cpnucleo.Domain.Commands.Responses.TipoTarefa
{
    public class CreateTipoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.TipoTarefa TipoTarefa { get; set; }
    }
}
