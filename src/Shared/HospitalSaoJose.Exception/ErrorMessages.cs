namespace HospitalSaoJose.Exception;

public static class ErrorMessages
{
    public const string UNKNOWN_ERROR = "Erro desconhecido.";

    public const string VALIDATION_NAME_REQUIRED = "O nome é obrigatório.";
    public const string VALIDATION_NAME_MAX_LENGTH = "O nome deve ter no máximo 255 caracteres.";
    public const string VALIDATION_EMAIL_REQUIRED = "O e-mail é obrigatório.";
    public const string VALIDATION_EMAIL_INVALID = "O e-mail informado é inválido.";
    public const string VALIDATION_EMAIL_ALREADY_EXISTS = "Já existe um usuário cadastrado com este e-mail.";
    public const string VALIDATION_PASSWORD_REQUIRED = "A senha é obrigatória.";
    public const string VALIDATION_PASSWORD_MIN_LENGTH = "A senha deve ter no mínimo 8 caracteres.";
    public const string VALIDATION_CURRENT_PASSWORD_INVALID = "A senha atual está incorreta.";
    public const string VALIDATION_LOGIN_INVALID = "E-mail ou senha inválidos.";
    public const string VALIDATION_ACCESS_TOKEN_EXPIRED = "A sessão expirou. Faça login novamente.";
    public const string VALIDATION_ACCESS_DENIED = "Você não tem permissão para executar esta ação.";

    public const string VALIDATION_PROFILE_REQUIRED = "O perfil é obrigatório.";
    public const string VALIDATION_PROFILE_NAME_REQUIRED = "O nome do perfil é obrigatório.";
    public const string VALIDATION_PROFILE_NAME_MAX_LENGTH = "O nome do perfil deve ter no máximo 100 caracteres.";
    public const string VALIDATION_PROFILE_NAME_ALREADY_EXISTS = "Já existe um perfil com este nome.";
    public const string VALIDATION_PROFILE_AT_LEAST_ONE_ROLE = "Selecione ao menos uma permissão para o perfil.";
    public const string VALIDATION_PROFILE_ROLE_NOT_FOUND = "Uma ou mais permissões informadas não existem.";
    public const string VALIDATION_PROFILE_HAS_ACTIVE_USERS = "Não é possível excluir um perfil com usuários ativos vinculados.";
    public const string VALIDATION_PROFILE_IS_SYSTEM = "Perfis de sistema não podem ser alterados ou excluídos.";
    public const string PROFILE_NOT_FOUND = "Perfil não encontrado.";

    public const string VALIDATION_ROLE_KEY_REQUIRED = "A chave da permissão é obrigatória.";
    public const string VALIDATION_ROLE_KEY_MAX_LENGTH = "A chave da permissão deve ter no máximo 100 caracteres.";
    public const string VALIDATION_ROLE_KEY_INVALID_FORMAT = "A chave da permissão deve seguir o formato 'recurso:acao'.";
    public const string VALIDATION_ROLE_KEY_ALREADY_EXISTS = "Já existe uma permissão com esta chave.";
    public const string VALIDATION_ROLE_NAME_REQUIRED = "O nome da permissão é obrigatório.";
    public const string VALIDATION_ROLE_NAME_MAX_LENGTH = "O nome da permissão deve ter no máximo 150 caracteres.";
    public const string VALIDATION_ROLE_IS_SYSTEM = "Permissões de sistema não podem ser alteradas ou excluídas.";
    public const string VALIDATION_ROLE_IN_USE = "Não é possível excluir uma permissão vinculada a um perfil.";
    public const string ROLE_NOT_FOUND = "Permissão não encontrada.";

    public const string VALIDATION_CATEGORY_NAME_REQUIRED = "O nome da categoria é obrigatório.";
    public const string VALIDATION_CATEGORY_NAME_MAX_LENGTH = "O nome da categoria deve ter no máximo 150 caracteres.";
    public const string VALIDATION_CATEGORY_SLUG_REQUIRED = "O identificador da categoria é obrigatório.";
    public const string VALIDATION_CATEGORY_SLUG_MAX_LENGTH = "O identificador da categoria deve ter no máximo 60 caracteres.";
    public const string VALIDATION_CATEGORY_SLUG_INVALID_FORMAT = "O identificador da categoria deve conter apenas letras minúsculas, números e hífen.";
    public const string VALIDATION_CATEGORY_SLUG_ALREADY_EXISTS = "Já existe uma categoria com este identificador.";
    public const string VALIDATION_CATEGORY_HAS_ACTIVE_DOCUMENTS = "Não é possível excluir uma categoria com documentos ativos.";
    public const string CATEGORY_NOT_FOUND = "Categoria não encontrada.";

    public const string VALIDATION_TITLE_REQUIRED = "O título é obrigatório.";
    public const string VALIDATION_TITLE_MAX_LENGTH = "O título deve ter no máximo 255 caracteres.";
    public const string VALIDATION_DESCRIPTION_REQUIRED = "A descrição é obrigatória.";
    public const string VALIDATION_DESCRIPTION_MAX_LENGTH = "A descrição deve ter no máximo 2000 caracteres.";
    public const string VALIDATION_EXTERNAL_LINK_INVALID = "O link externo informado é inválido.";
    public const string VALIDATION_PUBLICATION_DATE_REQUIRED = "A data de publicação é obrigatória.";
    public const string VALIDATION_CATEGORY_REQUIRED = "A categoria é obrigatória.";
    public const string VALIDATION_FILE_REQUIRED = "O arquivo é obrigatório.";
    public const string VALIDATION_FILE_TYPE_NOT_ACCEPTED = "Tipo de arquivo não aceito. Envie PDF, Word, Excel, PowerPoint, imagem ou ZIP.";
    public const string VALIDATION_FILE_CONTENT_MISMATCH = "O conteúdo do arquivo não corresponde à extensão do nome.";
    public const string VALIDATION_FILE_MAX_SIZE = "O arquivo deve ter no máximo 25 MB.";
    public const string DOCUMENT_NOT_FOUND = "Documento não encontrado.";
    public const string DOCUMENT_FILE_NOT_FOUND = "O arquivo deste documento não está disponível.";

    public const string USER_NOT_FOUND = "Usuário não encontrado.";
    public const string VALIDATION_PAGE_INVALID = "O número da página deve ser maior que zero.";
    public const string VALIDATION_PAGE_SIZE_INVALID = "A quantidade de itens por página deve estar entre 1 e 100.";
}
