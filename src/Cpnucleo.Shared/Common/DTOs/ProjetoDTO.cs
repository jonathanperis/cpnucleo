namespace Cpnucleo.Shared.Common.Dtos;

public sealed record ProjetoDto : BaseDto
{
    public string? Nome { get; set; }

    public Guid IdSistema { get; set; }

    public SistemaDto? Sistema { get; set; }
}