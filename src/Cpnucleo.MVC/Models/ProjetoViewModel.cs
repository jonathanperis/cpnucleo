using System.Collections.Generic;
using Cpnucleo.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cpnucleo.MVC.Models
{
    public class ProjetoViewModel
    {
        public Projeto Projeto { get; set; }

        public IEnumerable<Projeto> Lista { get; set; }

        public SelectList SelectSistemas { get; set; }
    }
}