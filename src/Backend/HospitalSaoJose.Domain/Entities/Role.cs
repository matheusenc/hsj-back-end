namespace HospitalSaoJose.Domain.Entities;

public sealed class Role : EntityBase
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsSystem { get; set; }

    public ICollection<ProfileRole> ProfileRoles { get; set; } = [];
}
