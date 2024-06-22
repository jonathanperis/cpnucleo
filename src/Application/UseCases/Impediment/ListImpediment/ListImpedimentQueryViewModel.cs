namespace Application.UseCases.Impediment.ListImpediment;

public sealed record ListImpedimentQueryViewModel(OperationResult OperationResult, List<ImpedimentDto> Impediments);
