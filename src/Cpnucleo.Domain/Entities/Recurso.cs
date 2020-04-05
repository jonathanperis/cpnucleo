namespace Cpnucleo.Domain.Entities
{
    public class Recurso : BaseModel
    {
        public string Nome { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        public string ConfirmarSenha { get; set; }

        public string Salt { get; set; }

        public string Token { get; set; }
    }
}