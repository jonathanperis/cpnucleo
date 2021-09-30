namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.UpdateTipoTarefa
{
    [DataContract]
    public class UpdateTipoTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
