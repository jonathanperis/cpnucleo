namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.RecursoProjeto;

public class UpdateRecursoProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdProjeto { get; set; }
}
