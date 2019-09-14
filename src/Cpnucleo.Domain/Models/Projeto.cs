using System;

namespace Cpnucleo.Domain.Models
{
    public class Projeto : BaseModel
    {
        public string Nome { get; set; }

        public Guid IdSistema { get; set; }

        public Sistema Sistema { get; set; }
    }
}