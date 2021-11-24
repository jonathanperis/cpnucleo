namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto;

public class CreateProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public ProjetoViewModel Projeto { get; set; }
}