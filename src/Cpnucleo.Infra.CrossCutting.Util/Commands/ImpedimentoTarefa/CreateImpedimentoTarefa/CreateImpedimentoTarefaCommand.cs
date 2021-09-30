namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa
{
    [DataContract]
    public class CreateImpedimentoTarefaCommand
    {
        [DataMember(Order = 1)]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
    }
}
