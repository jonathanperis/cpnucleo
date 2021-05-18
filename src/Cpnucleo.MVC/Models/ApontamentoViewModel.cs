using System.Collections.Generic;
using Cpnucleo.Domain.Entities;

namespace Cpnucleo.MVC.Models
{
    public class ApontamentoViewModel
    {
        public Apontamento Apontamento { get; set; }

        public IEnumerable<Apontamento> Lista { get; set; }

        public IEnumerable<Tarefa> ListaTarefas { get; set; }

        public IEnumerable<Workflow> ListaWorkflow { get; set; }
    }
}