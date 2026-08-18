namespace HospitalSaoJose.Api.Security;

internal static class PermissionPolicy
{
    internal const string PREFIX = "permission:";

    internal static string NameFor(string permission) => $"{PREFIX}{permission}";
}
