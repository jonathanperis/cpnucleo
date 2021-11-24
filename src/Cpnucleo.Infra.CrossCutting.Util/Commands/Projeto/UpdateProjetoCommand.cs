namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto;

public class UpdateProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public ProjetoViewModel Projeto { get; set; }
}