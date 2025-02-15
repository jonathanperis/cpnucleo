namespace Application.UseCases.Impediment.CreateImpediment;

public sealed class CreateImpedimentCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateImpedimentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateImpedimentCommand request, CancellationToken cancellationToken)
    {
        var impediment = Domain.Entities.Impediment.Create(request.Name, request.Id);

        if (dbContext.Impediments is not null)
            await dbContext.Impediments.AddAsync(impediment, cancellationToken);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
