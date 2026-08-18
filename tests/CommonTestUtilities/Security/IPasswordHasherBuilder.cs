using CommonTestUtilities.Cryptography;
using HospitalSaoJose.Domain.Security.PasswordHashing;
using Moq;

namespace CommonTestUtilities.Security;

public static class IPasswordHasherBuilder
{
    public static IPasswordHasher Build()
    {
        var hasher = new Mock<IPasswordHasher>();

        hasher.Setup(item => item.HashPassword(It.IsAny<string>()))
            .Returns<string>(PasswordHasherFake.Hash);

        hasher.Setup(item => item.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns<string, string>(PasswordHasherFake.Verify);

        return hasher.Object;
    }
}
