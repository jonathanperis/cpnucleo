namespace Cpnucleo.Application.Commands.UpdateTipoTarefa;

public sealed class UpdateTipoTarefaCommandValidator : AbstractValidator<UpdateTipoTarefaCommand>
{
    public UpdateTipoTarefaCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Tipo Tarefa");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Necessário informar o Nome do Tipo Tarefa");

        RuleFor(x => x.Nome)
            .MaximumLength(50)
            .WithMessage("Nome pode conter no máximo 50 caractéres");

        RuleFor(x => x.Image)
            .NotEmpty()
            .WithMessage("Necessário informar a Imagem do Tipo Tarefa");

        RuleFor(x => x.Image)
            .MaximumLength(50)
            .WithMessage("Imagem pode conter no máximo 50 caractéres");
    }
}
