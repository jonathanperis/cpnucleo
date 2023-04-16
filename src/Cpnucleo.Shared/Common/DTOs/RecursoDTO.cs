namespace Cpnucleo.Shared.Common.Dtos;

public sealed record RecursoDto(string? Nome, string? Login, string? Senha, string? ConfirmarSenha) : BaseDto;