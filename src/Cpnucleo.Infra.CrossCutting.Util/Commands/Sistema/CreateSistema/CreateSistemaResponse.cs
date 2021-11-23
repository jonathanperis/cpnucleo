namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;

public class CreateSistemaResponse : BaseCommand
{
    public OperationResult Status { get; set; }

    public SistemaViewModel Sistema { get; set; }
}