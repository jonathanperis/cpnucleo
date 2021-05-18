namespace Cpnucleo.Domain.Commands.Responses.ImpedimentoTarefa
{
    public class CreateImpedimentoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.ImpedimentoTarefa ImpedimentoTarefa { get; set; }
    }
}
