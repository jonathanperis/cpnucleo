namespace Application.UseCases.Impediment.ListImpediment;

public sealed record ListImpedimentQueryViewModel(OperationResult OperationResult, PaginatedResult<ImpedimentDto?>? Result);
