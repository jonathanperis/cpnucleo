namespace Cpnucleo.Infra.CrossCutting.Util.Common.DTOs;

public class RecursoDTO : BaseDTO
{
    public string Nome { get; set; }

    public string Login { get; set; }

    public string? Senha { get; set; }

    public string? ConfirmarSenha { get; set; }
}