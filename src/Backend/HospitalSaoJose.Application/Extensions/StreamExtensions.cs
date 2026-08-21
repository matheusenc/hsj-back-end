namespace HospitalSaoJose.Application.Extensions;

public static class StreamExtensions
{
    extension(Stream stream)
    {
        /// <summary>
        /// Compara os bytes do arquivo a partir de <paramref name="offset"/> com
        /// <paramref name="expected"/>. Deixa a posição onde encontrou, porque o
        /// mesmo stream é lido depois para gravar o arquivo.
        /// </summary>
        internal bool StartsWith(int offset, byte[] expected)
        {
            if (stream.CanSeek.Equals(false))
                return false;

            var buffer = new byte[offset + expected.Length];

            var posicaoOriginal = stream.Position;
            stream.Position = 0;
            var bytesRead = stream.ReadAtLeast(buffer, buffer.Length, throwOnEndOfStream: false);
            stream.Position = posicaoOriginal;

            return bytesRead == buffer.Length && buffer.AsSpan(offset).SequenceEqual(expected);
        }
    }
}
