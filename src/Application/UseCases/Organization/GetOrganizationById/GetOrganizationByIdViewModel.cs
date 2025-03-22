namespace Application.UseCases.Organization.GetOrganizationById;

public sealed record GetOrganizationByIdQueryViewModel(OperationResult OperationResult, OrganizationDto? Organization);
