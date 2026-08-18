using System.Text;

namespace CommonTestUtilities.Files;

public static class FileBuilder
{
    public static MemoryStream Pdf(int sizeInBytes = 1024)
    {
        var content = new byte[sizeInBytes];

        // Assinatura "%PDF-1.7" que o StreamExtensions.IsPdf() procura.
        var header = Encoding.ASCII.GetBytes("%PDF-1.7");
        Array.Copy(header, content, Math.Min(header.Length, content.Length));

        return new MemoryStream(content);
    }

    public static MemoryStream NotAPdf() => new(Encoding.ASCII.GetBytes("isto nao e um pdf"));
}
