namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa
{
    [DataContract]
    public class CreateImpedimentoTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
    }
}
