namespace Domain.Entities;

[Table("Tenants")]
public sealed class Tenant : BaseEntity
{
    public string Slug { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    public static Tenant Create(string slug, string name, Guid id = default)
    {
        return new Tenant
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Slug = Guard.Required(slug, nameof(Slug)),
            Name = Guard.Required(name, nameof(Name)),
            Active = true
        };
    }

    public static void Update(Tenant obj, string slug, string name)
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.Slug = Guard.Required(slug, nameof(Slug));
        obj.Name = Guard.Required(name, nameof(Name));
        obj.Touch();
    }

    public static void Remove(Tenant obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        obj.MarkRemoved();
    }
}
