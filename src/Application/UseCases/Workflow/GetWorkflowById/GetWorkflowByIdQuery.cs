namespace Application.UseCases.Workflow.GetWorkflowById;

public sealed record GetWorkflowByIdQuery(Ulid Id) : BaseQuery, IRequest<GetWorkflowByIdQueryViewModel>;
