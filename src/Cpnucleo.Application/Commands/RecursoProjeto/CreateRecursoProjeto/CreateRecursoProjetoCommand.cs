namespace Cpnucleo.Application.Commands.RecursoProjeto.CreateRecursoProjeto;

public class CreateRecursoProjetoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdProjeto { get; set; }
}
