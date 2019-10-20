using System;

namespace Cpnucleo.Domain.Models
{
    public abstract class BaseModel
    {
        public BaseModel()
        {
            Ativo = true;
        }

        public Guid Id { get; protected set; }

        public DateTime DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public DateTime? DataExclusao { get; set; }

        public bool Ativo { get; set; }
    }
}
