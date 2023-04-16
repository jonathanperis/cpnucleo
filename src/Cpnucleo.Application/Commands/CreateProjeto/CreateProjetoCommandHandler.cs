namespace Cpnucleo.Application.Commands.CreateProjeto;

public sealed class CreateProjetoCommandHandler : IRequestHandler<CreateProjetoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateProjetoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(CreateProjetoCommand request, CancellationToken cancellationToken)
    {
        var projeto = Projeto.Create(request.Nome, request.IdSistema);
        _context.Projetos.Add(projeto);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
