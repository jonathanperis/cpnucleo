namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;

public class CreateProjetoResponse : BaseCommand
{
    public OperationResult Status { get; set; }

    public ProjetoViewModel Projeto { get; set; }
}