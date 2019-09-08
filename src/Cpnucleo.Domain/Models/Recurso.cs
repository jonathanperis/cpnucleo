namespace Cpnucleo.Domain.Models
{
    public class Recurso : BaseModel
    {
        public Recurso() => Ativo = true;

        public string Nome { get; set; }

        public bool Ativo { get; set; }         

        public string Login { get; set; }        

        public string Senha { get; set; }

        public string Salt { get; set; }        
    }
}