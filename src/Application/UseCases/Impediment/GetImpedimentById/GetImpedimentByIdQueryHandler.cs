namespace Application.UseCases.Impediment.GetImpedimentById;
 
// EF Core
public sealed class GetImpedimentByIdQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetImpedimentByIdQuery, GetImpedimentByIdQueryViewModel>
{
    public async ValueTask<GetImpedimentByIdQueryViewModel> Handle(GetImpedimentByIdQuery request, CancellationToken cancellationToken)
    {
        var impediment = await dbContext.Impediments!
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);  
            
        var operationResult = impediment is not null ? OperationResult.Success : OperationResult.NotFound;

        return new GetImpedimentByIdQueryViewModel(operationResult, impediment?.MapToDto());
    }
}
