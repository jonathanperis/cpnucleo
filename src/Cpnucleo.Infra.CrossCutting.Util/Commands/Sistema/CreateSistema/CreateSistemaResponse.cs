namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema
{
    [DataContract]
    public class CreateSistemaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public SistemaViewModel Sistema { get; set; }
    }
}
