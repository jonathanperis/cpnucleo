﻿using Cpnucleo.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IRecursoTarefaAppService : IAppService<RecursoTarefaViewModel>
    {
        IEnumerable<RecursoTarefaViewModel> ListarPorTarefa(Guid idTarefa);

        IEnumerable<RecursoTarefaViewModel> ListarPorRecurso(Guid idRecurso);
    }
}