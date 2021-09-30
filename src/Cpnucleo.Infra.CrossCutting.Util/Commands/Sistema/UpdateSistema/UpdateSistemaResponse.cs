namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema
{
    [DataContract]
    public class UpdateSistemaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }
    }
}
