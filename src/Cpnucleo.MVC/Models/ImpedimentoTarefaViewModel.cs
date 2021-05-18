using System.Collections.Generic;
using Cpnucleo.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cpnucleo.MVC.Models
{
    public class ImpedimentoTarefaViewModel
    {
        public ImpedimentoTarefa ImpedimentoTarefa { get; set; }

        public IEnumerable<ImpedimentoTarefa> Lista { get; set; }

        public Tarefa Tarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }
    }
}