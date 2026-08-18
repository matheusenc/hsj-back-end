using HospitalSaoJose.Domain.Security.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace HospitalSaoJose.Api.Security;

internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var isSuperAdmin = context.User.HasClaim(TokenClaims.SuperAdmin, bool.TrueString);

        if (isSuperAdmin || context.User.HasClaim(TokenClaims.Permission, requirement.Permission))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
