namespace Cpnucleo.Shared.Commands.CreateApontamento;

public sealed class CreateApontamentoCommandValidator : AbstractValidator<CreateApontamentoCommand>
{
    public CreateApontamentoCommandValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty()
            .WithMessage("Necessário informar a Descrição do Apontamento");

        RuleFor(x => x.Descricao)
            .MaximumLength(450)
            .WithMessage("Descrição pode conter no máximo 450 caractéres");

        RuleFor(x => x.DataApontamento)
            .NotEmpty()
            .WithMessage("Necessário informar a Data do Apontamento");

        RuleFor(x => x.DataApontamento)
            .Must(x => x.Date < DateTime.UtcNow.Date)
            .WithMessage("Data do Apontamento não pode ser anterior ao dia atual");

        RuleFor(x => x.QtdHoras)
            .NotEmpty()
            .WithMessage("Necessário informar o Tempo Utilizado");

        RuleFor(x => x.QtdHoras)
            .InclusiveBetween(1, 24)
            .WithMessage("Tempo Utilizado deve estar entre 1 e 24");

        RuleFor(x => x.IdTarefa)
            .NotEmpty()
            .WithMessage("Apontamento deve conter uma Tarefa");

        RuleFor(x => x.IdRecurso)
            .NotEmpty()
            .WithMessage("Apontamento deve conter um Recurso");
    }
}
