namespace Cpnucleo.Shared.Commands.UpdateImpedimentoTarefa;

public sealed class UpdateImpedimentoTarefaCommandValidator : AbstractValidator<UpdateImpedimentoTarefaCommand>
{
    public UpdateImpedimentoTarefaCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Impedimento Tarefa");

        RuleFor(x => x.Descricao)
            .NotEmpty()
            .WithMessage("Necessário informar a Descrição do Impedimento Tarefa");

        RuleFor(x => x.Descricao)
            .MaximumLength(450)
            .WithMessage("Descrição pode conter no máximo 450 caractéres");

        RuleFor(x => x.IdTarefa)
            .NotEmpty()
            .WithMessage("Impedimento Tarefa deve conter uma Tarefa");

        RuleFor(x => x.IdImpedimento)
            .NotEmpty()
            .WithMessage("Impedimento Tarefa deve conter um Impedimento");
    }
}
