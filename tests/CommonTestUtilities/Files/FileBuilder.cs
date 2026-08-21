using System.Text;

namespace CommonTestUtilities.Files;

public static class FileBuilder
{
    public static MemoryStream Pdf(int sizeInBytes = 1024)
    {
        var content = new byte[sizeInBytes];

        // Assinatura "%PDF" que a lista de tipos aceitos confere.
        var header = Encoding.ASCII.GetBytes("%PDF-1.7");
        Array.Copy(header, content, Math.Min(header.Length, content.Length));

        return new MemoryStream(content);
    }

    /// <summary>Conteúdo que não bate com nenhuma assinatura conhecida.</summary>
    public static MemoryStream NotAPdf() => new(Encoding.ASCII.GetBytes("isto nao e um pdf"));

    public static MemoryStream Xlsx(int sizeInBytes = 1024)
    {
        var content = new byte[sizeInBytes];

        // Todo arquivo do Office moderno é um pacote ZIP: "PK".
        byte[] header = [0x50, 0x4B, 0x03, 0x04];
        Array.Copy(header, content, Math.Min(header.Length, content.Length));

        return new MemoryStream(content);
    }
}
