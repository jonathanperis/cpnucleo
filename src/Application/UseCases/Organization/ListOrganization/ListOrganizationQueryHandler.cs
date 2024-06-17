namespace Application.UseCases.Organization.ListOrganization;

public sealed class ListOrganizationQueryHandler(IOrganizationRepository organizationRepository) : IRequestHandler<ListOrganizationQuery, ListOrganizationQueryViewModel>
{
    public async ValueTask<ListOrganizationQueryViewModel> Handle(ListOrganizationQuery request, CancellationToken cancellationToken)
    {
        var organizations = await organizationRepository.ListOrganization();

        var operationResult = organizations is not null ? OperationResult.Success : OperationResult.NotFound;
        var organizationsList = organizations ?? [];  // Return an empty list if no systems are found

        return new ListOrganizationQueryViewModel(operationResult, organizationsList);
    }
}
