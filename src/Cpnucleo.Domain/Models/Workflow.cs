using System.Collections.Generic;

namespace Cpnucleo.Domain.Models
{
    public class Workflow : BaseModel
    {
        public string Nome { get; set; }

        public int? Ordem { get; set; }

        public List<Tarefa> ListaTarefas { get; set; }
    }
}