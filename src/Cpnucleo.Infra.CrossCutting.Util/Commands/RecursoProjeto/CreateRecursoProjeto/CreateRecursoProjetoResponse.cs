namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.CreateRecursoProjeto;

public class CreateRecursoProjetoResponse : BaseCommand
{
    public OperationResult Status { get; set; }

    public RecursoProjetoViewModel RecursoProjeto { get; set; }
}