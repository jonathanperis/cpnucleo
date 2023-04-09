namespace Cpnucleo.Application.Commands.UpdateRecursoProjeto;

public sealed class UpdateRecursoProjetoCommandHandler : IRequestHandler<UpdateRecursoProjetoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateRecursoProjetoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(UpdateRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        var recursoProjeto = await _context.RecursoProjetos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (recursoProjeto is null)
        {
            return OperationResult.NotFound;
        }

        recursoProjeto = Domain.Entities.RecursoProjeto.Update(recursoProjeto, request.IdRecurso, request.IdProjeto);
        _context.RecursoProjetos.Update(recursoProjeto);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
