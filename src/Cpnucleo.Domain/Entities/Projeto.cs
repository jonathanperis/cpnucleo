using System;

namespace Cpnucleo.Domain.Entities
{
    public class Projeto : BaseModel
    {
        public string Nome { get; set; }

        public Guid IdSistema { get; set; }

        public Sistema Sistema { get; set; }
    }
}