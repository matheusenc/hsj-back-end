using HospitalSaoJose.Domain.Entities;
using HospitalSaoJose.Domain.Security;
using HospitalSaoJose.Domain.Security.PasswordHashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalSaoJose.Infrastructure.DataAccess;

/// <summary>
/// Dados iniciais criados em código (e não em migration) porque o catálogo de permissões
/// vive em <see cref="Permissions.All"/> e a senha do primeiro admin depende do
/// <see cref="IPasswordHasher"/>. É idempotente: roda em todo startup sem duplicar nada.
/// </summary>
public static class DatabaseSeeder
{
    public const string ADMINISTRATOR_PROFILE_NAME = "Administrador";
    public const string EDITOR_PROFILE_NAME = "Editor";

    private static readonly (string Name, string Slug, int DisplayOrder)[] DefaultCategories =
    [
        ("Convênio Municipal", "convenio", 1),
        ("Convênio Ministério da Saúde", "conveniogov", 2),
        ("Emendas Parlamentares", "emenda", 3),
        ("Resoluções / Deliberações / Portarias", "resdelibport", 4)
    ];

    public static async Task Seed(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<HospitalSaoJoseDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var roles = await SeedRoles(dbContext);
        var administratorProfile = await SeedAdministratorProfile(dbContext, roles);
        await SeedEditorProfile(dbContext, roles);
        await SeedCategories(dbContext);
        await SeedAdministratorUser(dbContext, passwordHasher, configuration, administratorProfile);
    }

    private static async Task<List<Role>> SeedRoles(HospitalSaoJoseDbContext dbContext)
    {
        var existingRoles = await dbContext.Roles.ToListAsync();
        var existingKeys = existingRoles.Select(role => role.Key).ToHashSet();

        var missingRoles = Permissions.All
            .Where(permission => existingKeys.Contains(permission.Key).Equals(false))
            .Select(permission => new Role
            {
                Key = permission.Key,
                Name = permission.Name,
                Description = permission.Description,
                IsSystem = true
            })
            .ToList();

        if (missingRoles.Count > 0)
        {
            await dbContext.Roles.AddRangeAsync(missingRoles);
            await dbContext.SaveChangesAsync();
            existingRoles.AddRange(missingRoles);
        }

        return existingRoles;
    }

    private static async Task<Profile> SeedAdministratorProfile(HospitalSaoJoseDbContext dbContext, List<Role> roles)
    {
        var profile = await dbContext.Profiles
            .Include(item => item.ProfileRoles)
            .FirstOrDefaultAsync(item => item.Name == ADMINISTRATOR_PROFILE_NAME);

        if (profile is null)
        {
            profile = new Profile
            {
                Name = ADMINISTRATOR_PROFILE_NAME,
                Description = "Acesso total ao painel administrativo.",
                IsSystem = true
            };

            await dbContext.Profiles.AddAsync(profile);
        }

        // O perfil de sistema sempre recebe as permissões novas do catálogo.
        var linkedRoleIds = profile.ProfileRoles.Select(profileRole => profileRole.RoleId).ToHashSet();

        foreach (var role in roles.Where(role => linkedRoleIds.Contains(role.Id).Equals(false)))
            profile.ProfileRoles.Add(new ProfileRole { ProfileId = profile.Id, RoleId = role.Id });

        await dbContext.SaveChangesAsync();

        return profile;
    }

    private static async Task SeedEditorProfile(HospitalSaoJoseDbContext dbContext, List<Role> roles)
    {
        var editorExists = await dbContext.Profiles.AnyAsync(profile => profile.Name == EDITOR_PROFILE_NAME);

        if (editorExists)
            return;

        var editorRoleKeys = new[] { Permissions.DocumentsCreate, Permissions.DocumentsUpdate, Permissions.DocumentsDelete };

        var profile = new Profile
        {
            Name = EDITOR_PROFILE_NAME,
            Description = "Publica e mantém os documentos de transparência."
        };

        foreach (var role in roles.Where(role => editorRoleKeys.Contains(role.Key)))
            profile.ProfileRoles.Add(new ProfileRole { ProfileId = profile.Id, RoleId = role.Id });

        await dbContext.Profiles.AddAsync(profile);
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedCategories(HospitalSaoJoseDbContext dbContext)
    {
        if (await dbContext.Categories.AnyAsync())
            return;

        var categories = DefaultCategories.Select(category => new Category
        {
            Name = category.Name,
            Slug = category.Slug,
            DisplayOrder = category.DisplayOrder
        });

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedAdministratorUser(
        HospitalSaoJoseDbContext dbContext,
        IPasswordHasher passwordHasher,
        IConfiguration configuration,
        Profile administratorProfile)
    {
        if (await dbContext.Users.AnyAsync())
            return;

        var email = configuration["Seed:AdminEmail"];
        var password = configuration["Seed:AdminPassword"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;

        var user = new User
        {
            Name = configuration["Seed:AdminName"] ?? "Administrador",
            Email = email,
            Password = passwordHasher.HashPassword(password),
            ProfileId = administratorProfile.Id
        };

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }
}
