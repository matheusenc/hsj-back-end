using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace HospitalSaoJose.Api.Converters;

/// <summary>
/// Normaliza toda string que chega no corpo da requisição: remove espaços nas pontas
/// e colapsa espaços internos repetidos.
/// </summary>
public sealed partial class StringConverter : JsonConverter<string>
{
    [GeneratedRegex(@"\s+")]
    private static partial Regex WhiteSpaceRegex();

    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return WhiteSpaceRegex().Replace(value, " ").Trim();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) => writer.WriteStringValue(value);
}
