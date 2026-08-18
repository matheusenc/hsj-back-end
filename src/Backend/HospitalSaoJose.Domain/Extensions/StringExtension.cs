namespace HospitalSaoJose.Domain.Extensions;

public static class StringExtension
{
    extension(string? value)
    {
        public bool IsEmpty() => string.IsNullOrWhiteSpace(value);

        public bool IsNotEmpty() => !string.IsNullOrWhiteSpace(value);
    }
}
