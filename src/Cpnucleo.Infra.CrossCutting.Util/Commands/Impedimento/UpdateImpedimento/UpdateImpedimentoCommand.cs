namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento;

[DataContract]
public class UpdateImpedimentoCommand
{
    [DataMember(Order = 1)]
    public ImpedimentoViewModel Impedimento { get; set; }
}