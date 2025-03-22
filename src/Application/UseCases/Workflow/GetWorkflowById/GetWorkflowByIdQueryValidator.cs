namespace Application.UseCases.Workflow.GetWorkflowById;

public sealed class GetWorkflowByIdQueryValidator : AbstractValidator<GetWorkflowByIdQuery>
{
    public GetWorkflowByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
