namespace Application.UseCases.Impediment.ListImpediment;

public sealed class ListImpedimentQueryHandler(IImpedimentRepository impedimentRepository) : IRequestHandler<ListImpedimentQuery, ListImpedimentQueryViewModel>
{
    public async ValueTask<ListImpedimentQueryViewModel> Handle(ListImpedimentQuery request, CancellationToken cancellationToken)
    {
        var impediments = await impedimentRepository.ListImpediments();

        var operationResult = impediments is not null ? OperationResult.Success : OperationResult.NotFound;

        var result = impediments?
                                            .Select(x => x?.MapToDto())
                                            .ToList();

        return new ListImpedimentQueryViewModel(operationResult, result);
    }
}
