namespace Cpnucleo.Application.Commands.RemoveRecurso;

public sealed class RemoveRecursoCommandHandler : IRequestHandler<RemoveRecursoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveRecursoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(RemoveRecursoCommand request, CancellationToken cancellationToken)
    {
        var recurso = await _context.Recursos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (recurso is null)
        {
            return OperationResult.NotFound;
        }

        recurso = Domain.Entities.Recurso.Remove(recurso);
        _context.Recursos.Update(recurso); //JONATHAN - Soft Delete.

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
