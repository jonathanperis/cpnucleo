namespace Cpnucleo.Application.Commands.Sistema;

public sealed class UpdateSistemaCommandValidator : AbstractValidator<UpdateSistemaCommand>
{
    public UpdateSistemaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
        RuleFor(x => x.Descricao).NotEmpty();
        RuleFor(x => x.Descricao).MaximumLength(450);
    }
}
