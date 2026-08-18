using CommonTestUtilities.Requests;
using HospitalSaoJose.Application.UseCases.Profile;
using HospitalSaoJose.Exception;
using Shouldly;

namespace Validators.Tests.Profile;

public class ProfileValidatorTests
{
    [Fact]
    public void Success()
    {
        var result = new ProfileValidator().Validate(RequestProfileJsonBuilder.Build());

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_WhenNameIsEmpty()
    {
        var request = RequestProfileJsonBuilder.Build();
        request.Name = string.Empty;

        var result = new ProfileValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_PROFILE_NAME_REQUIRED);
    }

    [Fact]
    public void Error_WhenNameExceedsMaxLength()
    {
        var request = RequestProfileJsonBuilder.Build();
        request.Name = new string('a', 101);

        var result = new ProfileValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_PROFILE_NAME_MAX_LENGTH);
    }

    [Fact]
    public void Error_WhenThereIsNoRole()
    {
        var request = RequestProfileJsonBuilder.Build([]);

        var result = new ProfileValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_PROFILE_AT_LEAST_ONE_ROLE);
    }
}
