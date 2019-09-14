using System;

namespace Cpnucleo.Domain.Models
{
    public class ImpedimentoTarefa : BaseModel
    {
        public string Descricao { get; set; }

        public bool Ativo { get; set; }

        public Guid IdTarefa { get; set; }

        public Guid IdImpedimento { get; set; }

        public Tarefa Tarefa { get; set; }

        public Impedimento Impedimento { get; set; }
    }
}