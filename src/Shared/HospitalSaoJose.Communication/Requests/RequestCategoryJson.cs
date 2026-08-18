namespace HospitalSaoJose.Communication.Requests;

public sealed class RequestCategoryJson
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
