namespace HospitalSaoJose.Communication.Requests;

public sealed class RequestFilterDocumentsJson
{
    public string? CategorySlug { get; set; }
    public string? Title { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
}
