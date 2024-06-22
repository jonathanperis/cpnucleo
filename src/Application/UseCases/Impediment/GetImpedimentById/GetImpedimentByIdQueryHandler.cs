namespace Application.UseCases.Impediment.GetImpedimentById;

public sealed class GetImpedimentByIdQueryHandler(IImpedimentRepository impedimentRepository) : IRequestHandler<GetImpedimentByIdQuery, GetImpedimentByIdQueryViewModel>
{
    public async ValueTask<GetImpedimentByIdQueryViewModel> Handle(GetImpedimentByIdQuery request, CancellationToken cancellationToken)
    {
        var impediment = await impedimentRepository.GetImpedimentById(request.Id);

        var operationResult = impediment is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetImpedimentByIdQueryViewModel(operationResult, impediment);
    }
}
