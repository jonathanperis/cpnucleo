﻿namespace Cpnucleo.Application.Commands.ImpedimentoTarefa;

public sealed class CreateImpedimentoTarefaCommandValidator : AbstractValidator<CreateImpedimentoTarefaCommand>
{
    public CreateImpedimentoTarefaCommandValidator()
    {
        RuleFor(x => x.Descricao).NotEmpty();
        RuleFor(x => x.Descricao).MaximumLength(450);
        RuleFor(x => x.IdTarefa).NotEmpty();
        RuleFor(x => x.IdImpedimento).NotEmpty();
    }
}
