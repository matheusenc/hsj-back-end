using CommonTestUtilities.Requests;
using HospitalSaoJose.Application.UseCases.Role.Register;
using HospitalSaoJose.Domain.Security;
using HospitalSaoJose.Exception;
using Shouldly;

namespace Validators.Tests.Role.Register;

public class RegisterRoleValidatorTests
{
    [Fact]
    public void Success()
    {
        var result = new RegisterRoleValidator().Validate(RequestRegisterRoleJsonBuilder.Build());

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_ForEveryKeyInTheCatalog()
    {
        // Garante que uma permissão criada pela API pode ter o mesmo formato das de sistema.
        foreach (var permission in Permissions.All)
        {
            var request = RequestRegisterRoleJsonBuilder.Build();
            request.Key = permission.Key;

            new RegisterRoleValidator().Validate(request).IsValid.ShouldBeTrue($"a chave {permission.Key} deveria ser válida");
        }
    }

    [Theory]
    [InlineData("semdoispontos")]
    [InlineData("Users:Read")]
    [InlineData("users read")]
    [InlineData("users:")]
    [InlineData(":read")]
    public void Error_WhenKeyFormatIsInvalid(string key)
    {
        var request = RequestRegisterRoleJsonBuilder.Build();
        request.Key = key;

        var result = new RegisterRoleValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_ROLE_KEY_INVALID_FORMAT);
    }

    [Fact]
    public void Error_WhenKeyIsEmpty()
    {
        var request = RequestRegisterRoleJsonBuilder.Build();
        request.Key = string.Empty;

        var result = new RegisterRoleValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_ROLE_KEY_REQUIRED);
    }

    [Fact]
    public void Error_WhenNameIsEmpty()
    {
        var request = RequestRegisterRoleJsonBuilder.Build();
        request.Name = string.Empty;

        var result = new RegisterRoleValidator().Validate(request);

        result.Errors.ShouldContain(error => error.ErrorMessage == ErrorMessages.VALIDATION_ROLE_NAME_REQUIRED);
    }
}
