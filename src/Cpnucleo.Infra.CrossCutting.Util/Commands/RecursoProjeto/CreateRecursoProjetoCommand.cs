namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto;

public class CreateRecursoProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public RecursoProjetoViewModel RecursoProjeto { get; set; }
}