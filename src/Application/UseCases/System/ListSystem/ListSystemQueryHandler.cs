namespace Application.UseCases.System.ListSystem;

public sealed class ListSystemQueryHandler : IRequestHandler<ListSystemQuery, ListSystemQueryViewModel>
{
    private readonly ISystemRepository _systemRepository;

    public ListSystemQueryHandler(ISystemRepository systemRepository)
    {
        _systemRepository = systemRepository;
    }

    public async ValueTask<ListSystemQueryViewModel> Handle(ListSystemQuery request, CancellationToken cancellationToken)
    {
        var systems = await _systemRepository.ListSystem();

        var operationResult = systems is not null ? OperationResult.Success : OperationResult.NotFound;
        var systemsList = systems ?? new List<SystemDto>();  // Return an empty list if no systems are found

        return new ListSystemQueryViewModel(operationResult, systemsList);
    }
}
