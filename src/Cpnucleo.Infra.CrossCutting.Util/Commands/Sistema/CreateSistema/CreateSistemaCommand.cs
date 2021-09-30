namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;

[DataContract]
public class CreateSistemaCommand
{
    [DataMember(Order = 1)]
    public SistemaViewModel Sistema { get; set; }
}