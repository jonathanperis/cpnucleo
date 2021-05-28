using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Cpnucleo.MVC.Models
{
    public class RecursoTarefaView
    {
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public IEnumerable<RecursoTarefaViewModel> Lista { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectRecursos { get; set; }
    }
}