namespace Application.UseCases.Impediment.ListImpediment;

public sealed class ListImpedimentQueryHandler(IImpedimentRepository impedimentRepository) : IRequestHandler<ListImpedimentQuery, ListImpedimentQueryViewModel>
{
    public async ValueTask<ListImpedimentQueryViewModel> Handle(ListImpedimentQuery request, CancellationToken cancellationToken)
    {
        var impediments = await impedimentRepository.ListImpediments();

        var operationResult = impediments is not null ? OperationResult.Success : OperationResult.NotFound;
        var impedimentList = impediments ?? [];  // Return an empty list if no impediments are found

        var result = impedimentList.Select(impediment => (ImpedimentDto)impediment).ToList();

        return new ListImpedimentQueryViewModel(operationResult, result);
    }
}
