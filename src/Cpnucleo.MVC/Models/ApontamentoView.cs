using System.Collections.Generic;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.MVC.Models
{
    public class ApontamentoView
    {
        public ApontamentoViewModel Apontamento { get; set; }

        public IEnumerable<ApontamentoViewModel> Lista { get; set; }

        public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

        public IEnumerable<WorkflowViewModel> ListaWorkflow { get; set; }
    }
}