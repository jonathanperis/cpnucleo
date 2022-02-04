namespace Cpnucleo.Application.Commands.Recurso.CreateRecurso;

public class CreateRecursoCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Login { get; }
    public string Senha { get; internal set; }
    internal string Salt { get; set; }

    public CreateRecursoCommand(Guid id, string nome, string login, string senha)
    {
        Id = id;
        Nome = nome;
        Login = login;
        Senha = senha;
    }
}
