using System.Collections.Generic;

namespace Cpnucleo.Domain.Entities
{
    public class Workflow : BaseModel
    {
        public string Nome { get; set; }

        public int? Ordem { get; set; }

        public string TamanhoColuna { get; set; }

        public IEnumerable<Tarefa> ListaTarefas { get; set; }
    }
}