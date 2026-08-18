using System.Security.Cryptography;
using System.Text;
using HospitalSaoJose.Domain.Security.PasswordHashing;
using Konscious.Security.Cryptography;

namespace HospitalSaoJose.Infrastructure.Security.PasswordHashing;

internal sealed class Argon2PasswordHasher : IPasswordHasher
{
    private const int DEGREE_OF_PARALLELISM = 1;
    private const int ITERATIONS = 2;
    private const int MEMORY_SIZE = 20 * 1024;
    private const int SALT_SIZE = 16;
    private const int HASH_SIZE = 32;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SALT_SIZE);
        var hash = ComputeHash(password, salt);

        var result = new byte[SALT_SIZE + HASH_SIZE];
        Buffer.BlockCopy(salt, 0, result, 0, SALT_SIZE);
        Buffer.BlockCopy(hash, 0, result, SALT_SIZE, HASH_SIZE);

        return Convert.ToBase64String(result);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        byte[] storedBytes;

        try
        {
            storedBytes = Convert.FromBase64String(passwordHash);
        }
        catch (FormatException)
        {
            return false;
        }

        if (storedBytes.Length != SALT_SIZE + HASH_SIZE)
            return false;

        var salt = storedBytes.AsSpan(0, SALT_SIZE).ToArray();
        var expectedHash = storedBytes.AsSpan(SALT_SIZE, HASH_SIZE).ToArray();

        return CryptographicOperations.FixedTimeEquals(ComputeHash(password, salt), expectedHash);
    }

    private static byte[] ComputeHash(string password, byte[] salt)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DEGREE_OF_PARALLELISM,
            Iterations = ITERATIONS,
            MemorySize = MEMORY_SIZE
        };

        return argon2.GetBytes(HASH_SIZE);
    }
}
