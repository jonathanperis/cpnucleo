namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa
{
    [DataContract]
    public class UpdateImpedimentoTarefaCommand
    {
        [DataMember(Order = 1)]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
    }
}
