namespace Application.UseCases.Workflow.GetWorkflowById;

public sealed record GetWorkflowByIdQuery(Guid Id) : BaseQuery, IRequest<GetWorkflowByIdQueryViewModel>;
