﻿using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoTarefa
{
    public class UpdateRecursoTarefaComand : IRequest<UpdateRecursoTarefaResponse>
    {
        public RecursoTarefaViewModel RecursoTarefa { get; set; }
    }
}