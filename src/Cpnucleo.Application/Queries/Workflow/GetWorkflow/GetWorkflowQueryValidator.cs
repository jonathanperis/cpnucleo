namespace Cpnucleo.Application.Queries.Workflow.GetWorkflow;

public class GetWorkflowQueryValidator : AbstractValidator<GetWorkflowQuery>
{
    public GetWorkflowQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
