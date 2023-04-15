namespace Cpnucleo.Application.Commands.UpdateApontamento;

public sealed class UpdateApontamentoCommandHandler : IRequestHandler<UpdateApontamentoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateApontamentoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(UpdateApontamentoCommand request, CancellationToken cancellationToken)
    {
        var apontamento = await _context.Apontamentos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (apontamento is null)
        {
            return OperationResult.NotFound;
        }

        apontamento = Apontamento.Update(apontamento, request.Descricao, request.DataApontamento, request.QtdHoras, request.IdTarefa, request.IdRecurso);
        _context.Apontamentos.Update(apontamento);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
