namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema;

public class CreateSistemaCommand : BaseCommand, IRequest<OperationResult>
{
    public SistemaViewModel Sistema { get; set; }
}