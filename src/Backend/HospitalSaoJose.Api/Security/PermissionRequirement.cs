using Microsoft.AspNetCore.Authorization;

namespace HospitalSaoJose.Api.Security;

internal sealed class PermissionRequirement : IAuthorizationRequirement
{
    internal string Permission { get; }

    internal PermissionRequirement(string permission) => Permission = permission;
}
