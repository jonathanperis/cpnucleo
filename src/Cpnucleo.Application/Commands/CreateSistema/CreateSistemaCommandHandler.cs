namespace Cpnucleo.Application.Commands.CreateSistema;

public sealed class CreateSistemaCommandHandler : IRequestHandler<CreateSistemaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateSistemaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(CreateSistemaCommand request, CancellationToken cancellationToken)
    {
        var sistema = Sistema.Create(request.Nome, request.Descricao);
        _context.Sistemas.Add(sistema);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
