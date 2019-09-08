using System;

namespace Cpnucleo.Domain.Models
{
    public class Apontamento : BaseModel
    {
        public string Descricao { get; set; }

        public DateTime? DataApontamento { get; set; }

        public int QtdHoras { get; set; }

        public int? PercentualConcluido { get; set; }

        public Guid IdTarefa { get; set; }

        public Guid IdRecurso { get; set; }

        public Tarefa Tarefa { get; set; }
    }
}