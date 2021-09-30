namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema
{
    [DataContract]
    public class UpdateSistemaCommand
    {
        [DataMember(Order = 1)]
        public SistemaViewModel Sistema { get; set; }
    }
}
