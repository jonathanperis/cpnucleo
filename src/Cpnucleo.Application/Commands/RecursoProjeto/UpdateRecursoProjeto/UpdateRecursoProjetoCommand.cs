namespace Cpnucleo.Application.Commands.RecursoProjeto.UpdateRecursoProjeto;

public class UpdateRecursoProjetoCommand : IRequest<OperationResult>
{
    public UpdateRecursoProjetoCommand(Guid id, Guid idRecurso, Guid idProjeto, bool ativo)
    {
        Id = id;
        IdRecurso = idRecurso;
        IdProjeto = idProjeto;
        Ativo = ativo;
    }

    public Guid Id { get; }
    public Guid IdRecurso { get; }
    public Guid IdProjeto { get; }
    public bool Ativo { get; }
}
