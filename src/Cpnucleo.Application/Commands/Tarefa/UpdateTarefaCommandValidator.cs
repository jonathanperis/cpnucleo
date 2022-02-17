namespace Cpnucleo.Application.Commands.Tarefa;

public class UpdateTarefaCommandValidator : AbstractValidator<UpdateTarefaCommand>
{
    public UpdateTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(450);
        RuleFor(x => x.DataInicio).NotEmpty();
        RuleFor(x => x.DataInicio).Must(x => x.Date < DateTime.Now.Date);
        RuleFor(x => x.DataTermino).NotEmpty();
        RuleFor(x => x.DataTermino).Must(x => x.Date < DateTime.Now.Date);
        RuleFor(x => x.QtdHoras).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Detalhe).NotEmpty();
        RuleFor(x => x.Detalhe).MaximumLength(1000);
        RuleFor(x => x.IdProjeto).NotEmpty();
        RuleFor(x => x.IdWorkflow).NotEmpty();
        RuleFor(x => x.IdRecurso).NotEmpty();
        RuleFor(x => x.IdTipoTarefa).NotEmpty();
    }
}
