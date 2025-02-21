namespace Application.UseCases.Organization.ListOrganization;

public sealed record ListOrganizationQueryViewModel(OperationResult OperationResult, PaginatedResult<OrganizationDto?> Result);
