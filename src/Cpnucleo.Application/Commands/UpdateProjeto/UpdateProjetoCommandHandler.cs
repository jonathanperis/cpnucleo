namespace Cpnucleo.Application.Commands.UpdateProjeto;

public sealed class UpdateProjetoCommandHandler : IRequestHandler<UpdateProjetoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjetoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(UpdateProjetoCommand request, CancellationToken cancellationToken)
    {
        var projeto = await _context.Projetos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (projeto is null)
        {
            return OperationResult.NotFound;
        }

        projeto = Projeto.Update(projeto, request.Nome, request.IdSistema);
        _context.Projetos.Update(projeto);

        var success = await _context.SaveChangesAsync(cancellationToken);

        var result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
