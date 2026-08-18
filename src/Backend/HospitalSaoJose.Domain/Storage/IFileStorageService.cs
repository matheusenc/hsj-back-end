namespace HospitalSaoJose.Domain.Storage;

public interface IFileStorageService
{
    Task<string> Upload(Stream file, string extension);

    Task<Stream?> Get(string storedFileName);

    Task Delete(string storedFileName);
}
