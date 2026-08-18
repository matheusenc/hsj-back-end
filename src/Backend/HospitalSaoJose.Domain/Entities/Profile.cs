namespace HospitalSaoJose.Domain.Entities;

public sealed class Profile : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsSystem { get; set; }

    public ICollection<ProfileRole> ProfileRoles { get; set; } = [];
    public ICollection<User> Users { get; set; } = [];
}
