namespace Domain.Entities;

[Table("Tenants")]
public sealed class Tenant : BaseEntity
{
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public static Tenant Create(string slug, string name, Guid id = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Slug is required.", nameof(slug));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        var tenant = new Tenant
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Slug = slug.Trim(),
            Name = name.Trim(),
            Active = true
        };

        return tenant;
    }

    public static void Update(Tenant obj, string slug, string name)
    {
        ArgumentNullException.ThrowIfNull(obj);

        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Slug is required.", nameof(slug));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        obj.Slug = slug.Trim();
        obj.Name = name.Trim();
        obj.UpdatedAt = DateTime.UtcNow;
    }

    public static void Remove(Tenant obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        if (!obj.Active)
        {
            return;
        }

        obj.Active = false;
        obj.DeletedAt ??= DateTime.UtcNow;
    }
}
