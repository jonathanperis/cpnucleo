namespace Cpnucleo.Application.Commands;

public sealed class RemoveProjetoCommandHandler : IRequestHandler<RemoveProjetoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveProjetoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(RemoveProjetoCommand request, CancellationToken cancellationToken)
    {
        var projeto = await _context.Projetos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (projeto is null)
        {
            return OperationResult.NotFound;
        }

        projeto = Projeto.Remove(projeto);
        _context.Projetos.Update(projeto); //JONATHAN - Soft Delete.

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
