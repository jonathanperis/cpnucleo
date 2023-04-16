namespace Cpnucleo.Shared.Commands.UpdateTarefaByWorkflow;

public sealed class UpdateTarefaByWorkflowCommandValidator : AbstractValidator<UpdateTarefaByWorkflowCommand>
{
    public UpdateTarefaByWorkflowCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id da Tarefa");

        RuleFor(x => x.IdWorkflow)
            .NotEmpty()
            .WithMessage("Tarefa deve conter um Workflow");
    }
}
