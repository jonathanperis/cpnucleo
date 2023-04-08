namespace Cpnucleo.Domain.Entities;

public sealed class TipoTarefa : BaseEntity
{
    public string? Nome { get; private set; }

    public string? Image { get; private set; }

    public static TipoTarefa Create(string nome, string image, Guid id = default)
    {
        return new TipoTarefa
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            Nome = nome,
            Image = image,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static TipoTarefa Update(TipoTarefa item, string nome, string image)
    {
        item.Nome = nome;
        item.Image = image;
        item.DataAlteracao = DateTime.UtcNow;

        return item;
    }

    public static TipoTarefa Remove(TipoTarefa item)
    {
        item.Ativo = false;
        item.DataExclusao = DateTime.UtcNow;

        return item;
    }
}
