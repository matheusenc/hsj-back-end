namespace HospitalSaoJose.Application.Extensions;

public static class StreamExtensions
{
    private static readonly byte[] PdfSignature = [0x25, 0x50, 0x44, 0x46]; // "%PDF"

    extension(Stream stream)
    {
        internal bool IsPdf()
        {
            if (stream.CanSeek.Equals(false))
                return false;

            var buffer = new byte[PdfSignature.Length];

            stream.Position = 0;
            var bytesRead = stream.ReadAtLeast(buffer, buffer.Length, throwOnEndOfStream: false);
            stream.Position = 0;

            return bytesRead == PdfSignature.Length && buffer.SequenceEqual(PdfSignature);
        }
    }
}
