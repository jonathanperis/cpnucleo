using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.MVC.Models
{
    public class SistemaView
    {
        public SistemaViewModel Sistema { get; set; }

        public IEnumerable<SistemaViewModel> Lista { get; set; }
    }
}