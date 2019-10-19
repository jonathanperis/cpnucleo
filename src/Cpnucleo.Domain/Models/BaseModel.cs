using System;

namespace Cpnucleo.Domain.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; protected set; }

        public DateTime? DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public bool Ativo { get; set; }
    }
}
