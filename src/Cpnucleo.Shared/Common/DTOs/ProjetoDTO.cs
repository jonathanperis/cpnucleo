namespace Cpnucleo.Shared.Common.Dtos;

public sealed record ProjetoDto(string? Nome, Guid IdSistema, SistemaDto? Sistema) : BaseDto;