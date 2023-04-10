namespace Cpnucleo.Application.Commands.RemoveRecursoProjeto;

public sealed class RemoveRecursoProjetoCommandHandler : IRequestHandler<RemoveRecursoProjetoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveRecursoProjetoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(RemoveRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        var recursoProjeto = await _context.RecursoProjetos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (recursoProjeto is null)
        {
            return OperationResult.NotFound;
        }

        recursoProjeto = RecursoProjeto.Remove(recursoProjeto);
        _context.RecursoProjetos.Update(recursoProjeto); //JONATHAN - Soft Delete.

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
