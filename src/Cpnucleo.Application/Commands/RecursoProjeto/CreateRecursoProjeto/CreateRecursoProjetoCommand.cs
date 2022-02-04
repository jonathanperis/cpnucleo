namespace Cpnucleo.Application.Commands.RecursoProjeto.CreateRecursoProjeto;

public class CreateRecursoProjetoCommand : IRequest<OperationResult>
{
    public CreateRecursoProjetoCommand(Guid id, Guid idRecurso, Guid idProjeto)
    {
        Id = id;
        IdRecurso = idRecurso;
        IdProjeto = idProjeto;
    }

    public Guid Id { get; }
    public Guid IdRecurso { get; }
    public Guid IdProjeto { get; }
}
