namespace Application.UseCases.Organization.UpdateOrganization;

// Dapper Repository Advanced
public sealed class UpdateOrganizationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrganizationCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
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
            
            Domain.Entities.Organization.Update(organization, request.Name, request.Description);
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
