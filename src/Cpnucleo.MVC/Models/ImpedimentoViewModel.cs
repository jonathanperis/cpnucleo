using System.Collections.Generic;
using Cpnucleo.Domain.Entities;

namespace Cpnucleo.MVC.Models
{
    public class ImpedimentoViewModel
    {
        public Impedimento Impedimento { get; set; }

        public IEnumerable<Impedimento> Lista { get; set; }
    }
}