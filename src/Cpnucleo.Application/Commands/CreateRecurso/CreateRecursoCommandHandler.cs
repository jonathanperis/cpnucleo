namespace Cpnucleo.Application.Commands.CreateRecurso;

public sealed class CreateRecursoCommandHandler : IRequestHandler<CreateRecursoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateRecursoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(CreateRecursoCommand request, CancellationToken cancellationToken)
    {
        var recurso = Domain.Entities.Recurso.Create(request.Nome, request.Login, request.Senha);
        _context.Recursos.Add(recurso);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
