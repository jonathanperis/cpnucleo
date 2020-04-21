using System;

namespace Cpnucleo.Domain.Entities
{
    public class RecursoTarefa : BaseEntity
    {
        public int PercentualTarefa { get; set; }

        public Guid IdRecurso { get; set; }

        public Guid IdTarefa { get; set; }

        public Recurso Recurso { get; set; }

        public Tarefa Tarefa { get; set; }
    }
}