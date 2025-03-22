namespace Application.UseCases.Organization.GetOrganizationById;

// Dapper Repository Advanced
public sealed class GetOrganizationByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetOrganizationByIdQuery, GetOrganizationByIdQueryViewModel>
{
    public async ValueTask<GetOrganizationByIdQueryViewModel> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
        var response = await repository.GetByIdAsync(request.Id);        
        
        var operationResult = response is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetOrganizationByIdQueryViewModel(operationResult, response?.MapToDto());
    }
}
