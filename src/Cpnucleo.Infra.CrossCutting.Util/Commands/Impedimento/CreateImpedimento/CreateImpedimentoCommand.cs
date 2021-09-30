namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;

[DataContract]
public class CreateImpedimentoCommand
{
    [DataMember(Order = 1)]
    public ImpedimentoViewModel Impedimento { get; set; }
}