namespace Cpnucleo.Shared.Queries.GetWorkflow;

public sealed class GetWorkflowQueryValidator : AbstractValidator<GetWorkflowQuery>
{
    public GetWorkflowQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
