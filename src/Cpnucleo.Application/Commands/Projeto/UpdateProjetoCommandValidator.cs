namespace Cpnucleo.Application.Commands.Projeto;

public class UpdateProjetoCommandValidator : AbstractValidator<UpdateProjetoCommand>
{
    public UpdateProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
        RuleFor(x => x.IdSistema).NotEmpty();
    }
}
