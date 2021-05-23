using System.Collections.Generic;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.MVC.Models
{
    public class SistemaView
    {
        public SistemaViewModel Sistema { get; set; }

        public IEnumerable<SistemaViewModel> Lista { get; set; }
    }
}