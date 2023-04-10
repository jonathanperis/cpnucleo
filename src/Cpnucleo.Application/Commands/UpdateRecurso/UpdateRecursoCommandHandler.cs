namespace Cpnucleo.Application.Commands.UpdateRecurso;

public sealed class UpdateRecursoCommandHandler : IRequestHandler<UpdateRecursoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateRecursoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(UpdateRecursoCommand request, CancellationToken cancellationToken)
    {
        var recurso = await _context.Recursos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (recurso is null)
        {
            return OperationResult.NotFound;
        }

        recurso = Recurso.Update(recurso, request.Nome, request.Senha);
        _context.Recursos.Update(recurso);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
