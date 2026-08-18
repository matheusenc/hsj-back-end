namespace HospitalSaoJose.Domain.Entities;

public sealed class ProfileRole
{
    public Guid ProfileId { get; set; }
    public Guid RoleId { get; set; }

    public Profile Profile { get; set; } = null!;
    public Role Role { get; set; } = null!;
}
