namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento;

[DataContract]
public class RemoveApontamentoCommand
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}