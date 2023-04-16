namespace Cpnucleo.Application.Commands;

public sealed class CreateSistemaCommandHandler : IRequestHandler<CreateSistemaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateSistemaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(CreateSistemaCommand request, CancellationToken cancellationToken)
    {
        var sistema = Sistema.Create(request.Nome, request.Descricao, request.Id);
        _context.Sistemas.Add(sistema);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
