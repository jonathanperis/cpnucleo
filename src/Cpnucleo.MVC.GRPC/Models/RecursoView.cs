using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.MVC.Models
{
    public class RecursoView
    {
        public RecursoViewModel Recurso { get; set; }

        public IEnumerable<RecursoViewModel> Lista { get; set; }
    }
}