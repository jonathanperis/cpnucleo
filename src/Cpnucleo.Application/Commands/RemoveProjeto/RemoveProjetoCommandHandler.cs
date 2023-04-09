namespace Cpnucleo.Application.Commands.RemoveProjeto;

public sealed class RemoveProjetoCommandHandler : IRequestHandler<RemoveProjetoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveProjetoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(RemoveProjetoCommand request, CancellationToken cancellationToken)
    {
        var projeto = await _context.Projetos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (projeto is null)
        {
            return OperationResult.NotFound;
        }

        projeto = Domain.Entities.Projeto.Remove(projeto);
        _context.Projetos.Update(projeto); //JONATHAN - Soft Delete.

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
