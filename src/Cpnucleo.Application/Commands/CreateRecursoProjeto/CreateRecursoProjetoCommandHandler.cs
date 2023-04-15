namespace Cpnucleo.Application.Commands.CreateRecursoProjeto;

public sealed class CreateRecursoProjetoCommandHandler : IRequestHandler<CreateRecursoProjetoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateRecursoProjetoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(CreateRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        var recursoProjeto = RecursoProjeto.Create(request.IdProjeto, request.IdRecurso);
        _context.RecursoProjetos.Add(recursoProjeto);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
