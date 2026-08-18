using HospitalSaoJose.Domain.Storage;

namespace HospitalSaoJose.Infrastructure.Storage;

internal sealed class LocalFileStorageService : IFileStorageService
{
    private readonly string _rootPath;

    public LocalFileStorageService(string rootPath)
    {
        _rootPath = rootPath;
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<string> Upload(Stream file, string extension)
    {
        var storedFileName = $"{Guid.CreateVersion7()}{extension}";

        await using var destination = new FileStream(FullPath(storedFileName), FileMode.CreateNew, FileAccess.Write);

        file.Position = 0;
        await file.CopyToAsync(destination);

        return storedFileName;
    }

    public Task<Stream?> Get(string storedFileName)
    {
        var fullPath = FullPath(storedFileName);

        if (File.Exists(fullPath).Equals(false))
            return Task.FromResult<Stream?>(null);

        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);

        return Task.FromResult<Stream?>(stream);
    }

    public Task Delete(string storedFileName)
    {
        var fullPath = FullPath(storedFileName);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }

    private string FullPath(string storedFileName) => Path.Combine(_rootPath, Path.GetFileName(storedFileName));
}
