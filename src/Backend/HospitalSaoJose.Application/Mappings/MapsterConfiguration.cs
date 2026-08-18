using System.Runtime.CompilerServices;
using HospitalSaoJose.Communication.Requests;
using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Domain.Entities;
using Mapster;

[assembly: InternalsVisibleTo("UseCases.Tests")]
[assembly: InternalsVisibleTo("Validators.Tests")]

namespace HospitalSaoJose.Application.Mappings;

internal static class MapsterConfiguration
{
    internal static void Configure()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private static void RequestToDomain()
    {
        TypeAdapterConfig<RequestRegisterUserJson, User>
            .NewConfig()
            .Ignore(destination => destination.Id)
            .Ignore(destination => destination.Password)
            .Ignore(destination => destination.Profile);

        TypeAdapterConfig<RequestCategoryJson, Category>
            .NewConfig()
            .Ignore(destination => destination.Id)
            .Ignore(destination => destination.Documents);

        TypeAdapterConfig<RequestRegisterRoleJson, Role>
            .NewConfig()
            .Ignore(destination => destination.Id)
            .Ignore(destination => destination.IsSystem)
            .Ignore(destination => destination.ProfileRoles);

        TypeAdapterConfig<RequestProfileJson, Profile>
            .NewConfig()
            .Ignore(destination => destination.Id)
            .Ignore(destination => destination.IsSystem)
            .Ignore(destination => destination.ProfileRoles)
            .Ignore(destination => destination.Users);

        TypeAdapterConfig<RequestDocumentJson, Document>
            .NewConfig()
            .Ignore(destination => destination.Id)
            .Ignore(destination => destination.Category)
            .Ignore(destination => destination.OriginalFileName)
            .Ignore(destination => destination.StoredFileName)
            .Ignore(destination => destination.ContentType)
            .Ignore(destination => destination.SizeInBytes)
            .Ignore(destination => destination.CreatedByUserId);
    }

    private static void DomainToResponse()
    {
        TypeAdapterConfig<Profile, ResponseProfileSummaryJson>.NewConfig();

        TypeAdapterConfig<Profile, ResponseProfileJson>
            .NewConfig()
            .Map(destination => destination.Roles, source => source.ProfileRoles.Select(profileRole => profileRole.Role));

        TypeAdapterConfig<Role, ResponseRoleSummaryJson>.NewConfig();

        TypeAdapterConfig<Role, ResponseRoleJson>
            .NewConfig()
            .Map(destination => destination.Profiles, source => source.ProfileRoles.Select(profileRole => profileRole.Profile));

        TypeAdapterConfig<User, ResponseUserJson>.NewConfig();

        TypeAdapterConfig<User, ResponseRegisteredUserJson>.NewConfig();

        TypeAdapterConfig<Category, ResponseCategoryJson>.NewConfig();

        TypeAdapterConfig<Document, ResponseDocumentJson>
            .NewConfig()
            .Map(destination => destination.FileName, source => source.OriginalFileName)
            .Map(destination => destination.DownloadUrl, source => DownloadUrl(source.Id));
    }

    internal static string DownloadUrl(Guid documentId) => $"/documents/{documentId}/download";
}
