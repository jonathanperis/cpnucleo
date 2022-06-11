﻿namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.RecursoProjeto;

public class CreateRecursoProjetoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdProjeto { get; set; }
}