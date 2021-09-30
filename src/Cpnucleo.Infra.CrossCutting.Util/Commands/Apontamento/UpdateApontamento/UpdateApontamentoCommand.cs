namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.UpdateApontamento;

[DataContract]
public class UpdateApontamentoCommand
{
    [DataMember(Order = 1)]
    public ApontamentoViewModel Apontamento { get; set; }
}