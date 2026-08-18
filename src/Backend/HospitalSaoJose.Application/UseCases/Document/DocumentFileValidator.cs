using FluentValidation.Results;
using HospitalSaoJose.Application.Extensions;
using HospitalSaoJose.Exception;

namespace HospitalSaoJose.Application.UseCases.Document;

internal static class DocumentFileValidator
{
    internal const long MAXIMUM_SIZE_IN_BYTES = 25 * 1024 * 1024;
    internal const string PDF_CONTENT_TYPE = "application/pdf";
    internal const string PDF_EXTENSION = ".pdf";

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

        if (file.Content.IsPdf().Equals(false))
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.VALIDATION_ONLY_PDF_ACCEPTED));
    }
}
