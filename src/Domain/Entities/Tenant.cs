namespace Domain.Entities;

[Table("Tenants")]
public sealed class Tenant : BaseEntity
{
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public static Tenant Create(string slug, string name, Guid id = default)
    {
        var tenant = new Tenant
        {
            Id = GetNewId(id),
            CreatedAt = DateTime.UtcNow,
            Slug = slug,
            Name = name,
            Active = true
        };

        return tenant;
    }

    public static void Update(Tenant obj, string slug, string name)
    {
        obj.Slug = slug;
        obj.Name = name;
        obj.UpdatedAt = DateTime.UtcNow;
    }

    public static void Remove(Tenant obj)
    {
        obj.Active = false;
        obj.DeletedAt = DateTime.UtcNow;
    }
}
