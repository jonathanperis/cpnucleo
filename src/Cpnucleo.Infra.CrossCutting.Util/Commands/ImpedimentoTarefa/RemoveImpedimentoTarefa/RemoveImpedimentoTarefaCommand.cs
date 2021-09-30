namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa
{
    [DataContract]
    public class RemoveImpedimentoTarefaCommand
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
