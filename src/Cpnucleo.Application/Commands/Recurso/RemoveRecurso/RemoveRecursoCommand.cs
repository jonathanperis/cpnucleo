namespace Cpnucleo.Application.Commands.Recurso.RemoveRecurso;

public class RemoveRecursoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
