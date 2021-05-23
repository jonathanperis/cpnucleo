using System.Collections.Generic;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.MVC.Models
{
    public class RecursoView
    {
        public RecursoViewModel Recurso { get; set; }

        public IEnumerable<RecursoViewModel> Lista { get; set; }
    }
}