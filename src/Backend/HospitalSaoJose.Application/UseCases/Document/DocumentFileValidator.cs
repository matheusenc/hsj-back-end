using FluentValidation.Results;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.Document;

internal static class DocumentFileValidator
{
    internal const long MAXIMUM_SIZE_IN_BYTES = 25 * 1024 * 1024;

    /// <summary>
    /// O arquivo não faz parte do request JSON, então é validado à parte e os erros
    /// são anexados ao mesmo <see cref="ValidationResult"/> do request.
    /// </summary>
    internal static void Validate(ValidationResult result, DocumentFile file)
    {
        if (file.SizeInBytes <= 0)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_FILE_REQUIRED));
            return;
        }

        if (file.SizeInBytes > MAXIMUM_SIZE_IN_BYTES)
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_FILE_MAX_SIZE));

        var tipo = DocumentFileTypes.Find(file.Extension());

        if (tipo is null)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_FILE_TYPE_NOT_ACCEPTED));
            return;
        }

        // A extensão é só o que o nome promete; a assinatura é o que o arquivo é.
        if (tipo.MatchesContent(file.Content).Equals(false))
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_FILE_CONTENT_MISMATCH));
    }
}
