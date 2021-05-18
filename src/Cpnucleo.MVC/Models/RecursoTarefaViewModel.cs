using System.Collections.Generic;
using Cpnucleo.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cpnucleo.MVC.Models
{
    public class RecursoTarefaViewModel
    {
        public RecursoTarefa RecursoTarefa { get; set; }

        public IEnumerable<RecursoTarefa> Lista { get; set; }

        public Tarefa Tarefa { get; set; }

        public SelectList SelectRecursos { get; set; }
    }
}