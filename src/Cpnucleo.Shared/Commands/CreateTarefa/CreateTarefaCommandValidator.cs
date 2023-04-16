namespace Cpnucleo.Shared.Commands.CreateTarefa;

public sealed class CreateTarefaCommandValidator : AbstractValidator<CreateTarefaCommand>
{
    public CreateTarefaCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Necessário informar o Nome da Tarefa");

        RuleFor(x => x.Nome)
            .MaximumLength(450)
            .WithMessage("Nome pode conter no máximo 450 caractéres");

        RuleFor(x => x.DataInicio)
            .NotEmpty()
            .WithMessage("Necessário informar a Data de Início");

        RuleFor(x => x.DataInicio)
            .Must(x => x.Date >= DateTime.UtcNow.Date)
            .WithMessage("Data de Início não pode ser anterior ao dia atual");

        RuleFor(x => x.DataTermino)
            .NotEmpty()
            .WithMessage("Necessário informar a Data de Término");

        RuleFor(x => x.DataTermino)
            .Must(x => x.Date >= DateTime.UtcNow.Date)
            .WithMessage("Data de Término não pode ser anterior ao dia atual");

        RuleFor(x => x.DataTermino)
            .GreaterThan(x => x.DataInicio)
            .WithMessage("Data de Término não pode ser anterior a Data de Início");

        RuleFor(x => x.QtdHoras)
            .NotEmpty()
            .WithMessage("Necessário informar o Tempo Utilizado da Tarefa");

        RuleFor(x => x.QtdHoras)
            .GreaterThan(1)
            .WithMessage("Tempo Utilizado deve ser maior do que 1 Hora");

        RuleFor(x => x.Detalhe)
            .NotEmpty()
            .WithMessage("Necessário informar o Detalhe do Apontamento");

        RuleFor(x => x.Detalhe)
            .MaximumLength(450)
            .WithMessage("Detalhe pode conter no máximo 450 caractéres");

        RuleFor(x => x.IdProjeto)
            .NotEmpty()
            .WithMessage("Tarefa deve conter um Projeto");

        RuleFor(x => x.IdWorkflow)
            .NotEmpty()
            .WithMessage("Tarefa deve conter um Workflow");

        RuleFor(x => x.IdRecurso)
            .NotEmpty()
            .WithMessage("Tarefa deve conter um Recurso");

        RuleFor(x => x.IdTipoTarefa)
            .NotEmpty()
            .WithMessage("Tarefa deve conter um Tipo Tarefa");
    }
}
