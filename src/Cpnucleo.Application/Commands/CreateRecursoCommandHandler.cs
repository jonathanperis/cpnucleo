namespace Cpnucleo.Application.Commands;

public sealed class CreateRecursoCommandHandler : IRequestHandler<CreateRecursoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateRecursoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(CreateRecursoCommand request, CancellationToken cancellationToken)
    {
        var recurso = Recurso.Create(request.Nome, request.Login, request.Senha);
        _context.Recursos.Add(recurso);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
