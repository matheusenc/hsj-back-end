using CommonTestUtilities.Requests;
using HospitalSaoJose.Application.UseCases.User.Register;
using HospitalSaoJose.Exception;
using Shouldly;

namespace Validators.Tests.User.Register;

public class RegisterUserValidatorTests
{
    [Fact]
    public void Success()
    {
        var result = new RegisterUserValidator().Validate(RequestRegisterUserJsonBuilder.Build());

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_WhenNameIsEmpty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = new RegisterUserValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_NAME_REQUIRED);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Error_WhenEmailIsEmpty(string email)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email;

        var result = new RegisterUserValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_EMAIL_REQUIRED);
    }

    [Fact]
    public void Error_WhenEmailIsInvalid()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "usuario@";

        var result = new RegisterUserValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_EMAIL_INVALID);
    }

    [Fact]
    public void Error_WhenPasswordIsTooShort()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = "Aa1!";

        var result = new RegisterUserValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_PASSWORD_MIN_LENGTH);
    }

    [Fact]
    public void Error_WhenProfileIsMissing()
    {
        var request = RequestRegisterUserJsonBuilder.Build(Guid.Empty);

        var result = new RegisterUserValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_PROFILE_REQUIRED);
    }
}
