namespace Cpnucleo.Application.Commands.RecursoProjeto.UpdateRecursoProjeto;

public class UpdateRecursoProjetoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdProjeto { get; set; }
}
