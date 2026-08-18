using Microsoft.AspNetCore.Authorization;

namespace HospitalSaoJose.Api.Security;

/// <summary>
/// Protege uma action exigindo uma permissão do catálogo <c>Permissions</c>.
/// A policy correspondente é criada sob demanda pelo <see cref="PermissionAuthorizationPolicyProvider"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) => Policy = PermissionPolicy.NameFor(permission);
}
