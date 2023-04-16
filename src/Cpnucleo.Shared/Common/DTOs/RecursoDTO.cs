namespace Cpnucleo.Shared.Common.Dtos;

public sealed record RecursoDto : BaseDto
{
    public string? Nome { get; set; }

    public string? Login { get; set; }

    public string? Senha { get; set; }

    public string? ConfirmarSenha { get; set; }
}