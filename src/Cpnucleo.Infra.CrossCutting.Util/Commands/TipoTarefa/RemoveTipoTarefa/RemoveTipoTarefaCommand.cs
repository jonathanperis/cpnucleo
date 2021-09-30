namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.RemoveTipoTarefa;

[DataContract]
public class RemoveTipoTarefaCommand
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}