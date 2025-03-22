namespace Application.UseCases.Impediment.GetImpedimentById;

public sealed record GetImpedimentByIdQueryViewModel(OperationResult OperationResult, ImpedimentDto? Impediment);
