namespace Cpnucleo.Application.Commands.Recurso.UpdateRecurso;

public class UpdateRecursoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Senha { get; set; }
}
