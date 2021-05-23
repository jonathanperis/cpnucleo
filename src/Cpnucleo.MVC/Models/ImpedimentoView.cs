using System.Collections.Generic;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.MVC.Models
{
    public class ImpedimentoView
    {
        public ImpedimentoViewModel Impedimento { get; set; }

        public IEnumerable<ImpedimentoViewModel> Lista { get; set; }
    }
}