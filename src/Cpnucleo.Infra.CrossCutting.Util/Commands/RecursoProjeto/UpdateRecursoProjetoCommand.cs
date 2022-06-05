namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto;

public class UpdateRecursoProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdProjeto { get; set; }
}
