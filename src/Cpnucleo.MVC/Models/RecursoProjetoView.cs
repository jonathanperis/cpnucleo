using System.Collections.Generic;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cpnucleo.MVC.Models
{
    public class RecursoProjetoView
    {
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public IEnumerable<RecursoProjetoViewModel> Lista { get; set; }

        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }
    }
}