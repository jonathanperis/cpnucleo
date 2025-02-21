namespace Application.UseCases.Organization.RemoveOrganization;

// Dapper Repository Advanced
public sealed class RemoveOrganizationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveOrganizationCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveOrganizationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync();
            
            var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
            var organization = await repository.GetByIdAsync(request.Id);

            if (organization is null)
            {
                return OperationResult.NotFound;
            }
            
            Domain.Entities.Organization.Remove(organization);
            var success = await repository.UpdateAsync(organization);    
        
            await unitOfWork.CommitAsync(cancellationToken);

            return success ? OperationResult.Success : OperationResult.Failed;
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
