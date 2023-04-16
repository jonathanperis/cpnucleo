namespace Cpnucleo.Application.Commands.CreateApontamento;

public sealed class CreateApontamentoCommandHandler : IRequestHandler<CreateApontamentoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateApontamentoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(CreateApontamentoCommand request, CancellationToken cancellationToken)
    {
        var apontamento = Apontamento.Create(request.Descricao, request.DataApontamento, request.QtdHoras, request.IdTarefa, request.IdRecurso);
        _context.Apontamentos.Add(apontamento);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
