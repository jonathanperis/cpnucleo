namespace Cpnucleo.Domain.Models
{
    public class Recurso : BaseModel
    {
        public string Nome { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        public string Salt { get; set; }
    }
}