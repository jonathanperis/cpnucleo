using System.Collections.Generic;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cpnucleo.MVC.Models
{
    public class ImpedimentoTarefaView
    {
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }

        public IEnumerable<ImpedimentoTarefaViewModel> Lista { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }
    }
}