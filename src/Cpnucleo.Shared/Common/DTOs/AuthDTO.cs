namespace Cpnucleo.Shared.Common.Dtos;

public sealed record AuthDto
{
    public string? Usuario { get; set; }

    public string? Senha { get; set; }
}