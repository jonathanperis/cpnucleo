using System;

namespace Cpnucleo.Domain.Models
{
    public class RecursoProjeto : BaseModel
    {
        public Guid IdRecurso { get; set; }

        public Guid IdProjeto { get; set; }

        public Recurso Recurso { get; set; }

        public Projeto Projeto { get; set; }
    }
}