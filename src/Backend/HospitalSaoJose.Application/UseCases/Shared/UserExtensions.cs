namespace HospitalSaoJose.Application.UseCases.Shared;

public static class UserExtensions
{
    extension(Domain.Entities.User user)
    {
        /// <summary>
        /// Exige que <c>Profile.ProfileRoles.Role</c> tenha sido carregado pelo repositório.
        /// </summary>
        internal List<string> Permissions() => user.Profile.ProfileRoles
            .Where(profileRole => profileRole.Role.Active)
            .Select(profileRole => profileRole.Role.Key)
            .ToList();

        internal bool IsSuperAdmin() => user.Profile.IsSystem;
    }
}
