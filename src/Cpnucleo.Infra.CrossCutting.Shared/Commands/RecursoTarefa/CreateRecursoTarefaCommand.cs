﻿namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.RecursoTarefa;

public class CreateRecursoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdTarefa { get; set; }
}