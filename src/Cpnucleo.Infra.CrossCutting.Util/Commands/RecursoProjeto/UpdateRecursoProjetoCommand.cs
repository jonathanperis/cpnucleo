﻿namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto;

public class UpdateRecursoProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public RecursoProjetoViewModel RecursoProjeto { get; set; }
}