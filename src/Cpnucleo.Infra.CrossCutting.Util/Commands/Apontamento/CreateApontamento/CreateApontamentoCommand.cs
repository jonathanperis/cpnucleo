namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;

[DataContract]
public class CreateApontamentoCommand
{
    [DataMember(Order = 1)]
    public ApontamentoViewModel Apontamento { get; set; }
}