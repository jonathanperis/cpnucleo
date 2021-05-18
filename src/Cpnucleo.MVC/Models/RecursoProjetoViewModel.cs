using System.Collections.Generic;
using Cpnucleo.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cpnucleo.MVC.Models
{
    public class RecursoProjetoViewModel
    {
        public RecursoProjeto RecursoProjeto { get; set; }

        public IEnumerable<RecursoProjeto> Lista { get; set; }

        public Projeto Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }
    }
}