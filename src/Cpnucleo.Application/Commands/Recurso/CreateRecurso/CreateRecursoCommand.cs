namespace Cpnucleo.Application.Commands.Recurso.CreateRecurso;

public class CreateRecursoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }
    internal string Salt { get; set; }
}
