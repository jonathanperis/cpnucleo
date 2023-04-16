namespace Cpnucleo.Shared.Common.Dtos;

public sealed record RecursoProjetoDto(Guid IdRecurso, Guid IdProjeto, RecursoDto? Recurso, ProjetoDto? Projeto) : BaseDto;