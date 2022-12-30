namespace Cpnucleo.Application.Commands.Apontamento;

public sealed class CreateApontamentoCommandValidator : AbstractValidator<CreateApontamentoCommand>
{
    public CreateApontamentoCommandValidator()
    {
        RuleFor(x => x.Descricao).NotEmpty();
        RuleFor(x => x.Descricao).MaximumLength(450);
        RuleFor(x => x.DataApontamento).NotEmpty();
        RuleFor(x => x.DataApontamento).Must(x => x.Date >= DateTime.UtcNow.Date);
        RuleFor(x => x.QtdHoras).NotEmpty();
        RuleFor(x => x.QtdHoras).InclusiveBetween(1, 24);
        RuleFor(x => x.IdTarefa).NotEmpty();
        RuleFor(x => x.IdRecurso).NotEmpty();
    }
}
