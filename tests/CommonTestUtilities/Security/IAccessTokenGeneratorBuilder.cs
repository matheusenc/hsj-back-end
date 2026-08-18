using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Security;

public static class IAccessTokenGeneratorBuilder
{
    public const string TOKEN = "token-de-teste";

    public static IAccessTokenGenerator Build()
    {
        var generator = new Mock<IAccessTokenGenerator>();

        generator
            .Setup(item => item.Generate(It.IsAny<User>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()))
            .Returns(new AccessToken(TOKEN, DateTime.UtcNow.AddHours(1)));

        return generator.Object;
    }
}
