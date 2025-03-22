namespace Application.UseCases.Organization.CreateOrganization;

// Dapper Repository Advanced
public sealed class CreateOrganizationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateOrganizationCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var system = Domain.Entities.Organization.Create(request.Name, request.Description, request.Id);

            await unitOfWork.BeginTransactionAsync();
        
            var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
            var response = await repository.AddAsync(system);    
        
            await unitOfWork.CommitAsync(cancellationToken);

            return response != Guid.Empty ? OperationResult.Success : OperationResult.Failed;
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            return OperationResult.Failed;
        }
    }
}
