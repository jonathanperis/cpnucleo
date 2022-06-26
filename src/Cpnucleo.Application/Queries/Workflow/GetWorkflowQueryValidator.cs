namespace Cpnucleo.Application.Queries.Workflow;

public class GetWorkflowQueryValidator : AbstractValidator<GetWorkflowQuery>
{
    public GetWorkflowQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
