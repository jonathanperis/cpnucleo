namespace Application.UseCases.System.GetSystemById;

public sealed class GetSystemByIdQueryHandler : IRequestHandler<GetSystemByIdQuery, GetSystemByIdQueryViewModel>
{
    private readonly ISystemRepository _systemRepository;

    public GetSystemByIdQueryHandler(ISystemRepository systemRepository)
    {
        _systemRepository = systemRepository;
    }

    public async ValueTask<GetSystemByIdQueryViewModel> Handle(GetSystemByIdQuery request, CancellationToken cancellationToken)
    {
        var system = await _systemRepository.GetSystemById(request.Id);

        var operationResult = system is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetSystemByIdQueryViewModel(operationResult, system);
    }
}
