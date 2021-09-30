namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.UpdateApontamento;

[DataContract]
public class UpdateApontamentoResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }
}