namespace Cpnucleo.Application.Commands.UpdateSistema;

public sealed class UpdateSistemaCommandHandler : IRequestHandler<UpdateSistemaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateSistemaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(UpdateSistemaCommand request, CancellationToken cancellationToken)
    {
        var sistema = await _context.Sistemas
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (sistema is null)
        {
            return OperationResult.NotFound;
        }

        sistema = Sistema.Update(sistema, request.Nome, request.Descricao);
        _context.Sistemas.Update(sistema);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
