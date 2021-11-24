namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema;

public class UpdateSistemaCommand : BaseCommand, IRequest<OperationResult>
{
    public SistemaViewModel Sistema { get; set; }
}