namespace Application.UseCases.Project.GetProjectById;

// Dapper Repository Basic
public sealed class GetProjectByIdQueryHandler(IProjectRepository repository) : IRequestHandler<GetProjectByIdQuery, GetProjectByIdQueryViewModel>
{
    public async ValueTask<GetProjectByIdQueryViewModel> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await repository.GetByIdAsync(request.Id);        
        
        var operationResult = response is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetProjectByIdQueryViewModel(operationResult, response?.MapToDto());
    }
}