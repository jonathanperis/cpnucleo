﻿namespace Cpnucleo.Application.Commands.RemoveApontamento;

public sealed class RemoveApontamentoCommandHandler : IRequestHandler<RemoveApontamentoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public RemoveApontamentoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(RemoveApontamentoCommand request, CancellationToken cancellationToken)
    {
        var apontamento = await _context.Apontamentos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (apontamento is null)
        {
            return OperationResult.NotFound;
        }

        apontamento = Domain.Entities.Apontamento.Remove(apontamento);
        _context.Apontamentos.Update(apontamento); //JONATHAN - Soft Delete.

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
