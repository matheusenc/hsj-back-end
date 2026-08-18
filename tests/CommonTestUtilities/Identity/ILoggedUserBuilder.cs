using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Identity;
using Moq;

namespace CommonTestUtilities.Identity;

public static class ILoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var loggedUser = new Mock<ILoggedUser>();

        loggedUser.Setup(item => item.Get()).ReturnsAsync(user);
        loggedUser.Setup(item => item.GetUserId()).Returns(user.Id);

        return loggedUser.Object;
    }
}
