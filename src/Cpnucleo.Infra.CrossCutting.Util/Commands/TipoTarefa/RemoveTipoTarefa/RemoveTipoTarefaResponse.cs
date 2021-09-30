namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.RemoveTipoTarefa;

[DataContract]
public class RemoveTipoTarefaResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }
}