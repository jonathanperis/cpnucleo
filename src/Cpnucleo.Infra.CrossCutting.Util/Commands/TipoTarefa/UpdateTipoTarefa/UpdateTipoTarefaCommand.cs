namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.UpdateTipoTarefa;

[DataContract]
public class UpdateTipoTarefaCommand
{
    [DataMember(Order = 1)]
    public TipoTarefaViewModel TipoTarefa { get; set; }
}