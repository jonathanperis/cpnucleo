namespace Cpnucleo.Application.Queries.RecursoTarefa.GetRecursoTarefa;

public class GetRecursoTarefaQueryValidator : AbstractValidator<GetRecursoTarefaQuery>
{
    public GetRecursoTarefaQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
