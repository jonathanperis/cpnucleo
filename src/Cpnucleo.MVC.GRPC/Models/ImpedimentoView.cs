using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.MVC.Models
{
    public class ImpedimentoView
    {
        public ImpedimentoViewModel Impedimento { get; set; }

        public IEnumerable<ImpedimentoViewModel> Lista { get; set; }
    }
}