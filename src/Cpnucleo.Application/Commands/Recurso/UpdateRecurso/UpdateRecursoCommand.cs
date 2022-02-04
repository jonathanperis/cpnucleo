namespace Cpnucleo.Application.Commands.Recurso.UpdateRecurso;

public class UpdateRecursoCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Senha { get; }

    public UpdateRecursoCommand(Guid id, string nome, string senha)
    {
        Id = id;
        Nome = nome;
        Senha = senha;
    }
}
