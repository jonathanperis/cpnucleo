using System.Collections.Generic;
using Cpnucleo.Domain.Entities;

namespace Cpnucleo.MVC.Models
{
    public class RecursoViewModel
    {
        public Recurso Recurso { get; set; }

        public IEnumerable<Recurso> Lista { get; set; }
    }
}