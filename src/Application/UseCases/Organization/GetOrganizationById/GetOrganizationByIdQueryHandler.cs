namespace Application.UseCases.Organization.GetOrganizationById;

public sealed class GetOrganizationByIdQueryHandler(IOrganizationRepository organizationRepository) : IRequestHandler<GetOrganizationByIdQuery, GetOrganizationByIdQueryViewModel>
{
    public async ValueTask<GetOrganizationByIdQueryViewModel> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var system = await organizationRepository.GetOrganizationById(request.Id);

        var operationResult = system is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetOrganizationByIdQueryViewModel(operationResult, system);
    }
}
