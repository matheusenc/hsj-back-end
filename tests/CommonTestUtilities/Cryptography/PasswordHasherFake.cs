namespace CommonTestUtilities.Cryptography;

/// <summary>
/// Hash previsível para os testes: evita o custo do Argon2 e permite montar entidades
/// com senha conhecida sem depender da Infrastructure.
/// </summary>
public static class PasswordHasherFake
{
    private const string PREFIX = "hash::";

    public static string Hash(string password) => $"{PREFIX}{password}";

    public static bool Verify(string password, string passwordHash) => Hash(password) == passwordHash;
}
