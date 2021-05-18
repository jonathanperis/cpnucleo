using System.Collections.Generic;
using Cpnucleo.Domain.Entities;

namespace Cpnucleo.MVC.Models
{
    public class SistemaViewModel
    {
        public Sistema Sistema { get; set; }

        public IEnumerable<Sistema> Lista { get; set; }
    }
}