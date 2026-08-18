namespace HospitalSaoJose.Domain.Security.Tokens;

public sealed record AccessToken(string Value, DateTime ExpiresAtUtc);
