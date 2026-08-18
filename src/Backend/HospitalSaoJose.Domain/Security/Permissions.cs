namespace HospitalSaoJose.Domain.Security;

public static class Permissions
{
    public const string UsersRead = "users:read";
    public const string UsersCreate = "users:create";
    public const string UsersUpdate = "users:update";
    public const string UsersDelete = "users:delete";

    public const string ProfilesRead = "profiles:read";
    public const string ProfilesCreate = "profiles:create";
    public const string ProfilesUpdate = "profiles:update";
    public const string ProfilesDelete = "profiles:delete";

    public const string RolesRead = "roles:read";
    public const string RolesCreate = "roles:create";
    public const string RolesUpdate = "roles:update";
    public const string RolesDelete = "roles:delete";

    public const string CategoriesCreate = "categories:create";
    public const string CategoriesUpdate = "categories:update";
    public const string CategoriesDelete = "categories:delete";

    public const string DocumentsCreate = "documents:create";
    public const string DocumentsUpdate = "documents:update";
    public const string DocumentsDelete = "documents:delete";

    public static IReadOnlyList<PermissionDefinition> All { get; } =
    [
        new(UsersRead, "Listar usuários", "Visualizar a lista de usuários do painel."),
        new(UsersCreate, "Cadastrar usuários", "Criar novos usuários e atribuir perfis."),
        new(UsersUpdate, "Editar usuários", "Alterar nome, e-mail e perfil de usuários."),
        new(UsersDelete, "Desativar usuários", "Desativar o acesso de usuários ao painel."),

        new(ProfilesRead, "Listar perfis", "Visualizar os perfis e suas permissões."),
        new(ProfilesCreate, "Cadastrar perfis", "Criar novos perfis de acesso."),
        new(ProfilesUpdate, "Editar perfis", "Alterar dados e permissões de um perfil."),
        new(ProfilesDelete, "Excluir perfis", "Desativar perfis de acesso."),

        new(RolesRead, "Listar permissões", "Visualizar o catálogo de permissões."),
        new(RolesCreate, "Cadastrar permissões", "Criar novas permissões."),
        new(RolesUpdate, "Editar permissões", "Alterar nome e descrição de permissões."),
        new(RolesDelete, "Excluir permissões", "Desativar permissões."),

        new(CategoriesCreate, "Cadastrar categorias", "Criar categorias de documentos."),
        new(CategoriesUpdate, "Editar categorias", "Alterar categorias de documentos."),
        new(CategoriesDelete, "Excluir categorias", "Desativar categorias de documentos."),

        new(DocumentsCreate, "Publicar documentos", "Enviar novos documentos de transparência."),
        new(DocumentsUpdate, "Editar documentos", "Alterar documentos já publicados."),
        new(DocumentsDelete, "Excluir documentos", "Remover documentos publicados.")
    ];
}
