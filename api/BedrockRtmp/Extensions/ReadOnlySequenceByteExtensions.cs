using System;
using System.Buffers;
using System.Text;

namespace BedrockRtmp.Extensions
{
    internal static class ReadOnlySequenceByteExtensions
    {
        public static bool TryReadLine(this ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
        {
            var position = buffer.PositionOf((byte)'\n');
            if (position == null)
            {
                line = default;
                return false;
            }

            line = buffer.Slice(0, position.Value);
            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
            return true;
        }

        public static string ParseAsUTF8String(this ref ReadOnlySequence<byte> buffer, int estimate = -1)
        {
            if (estimate == -1) estimate = (int)buffer.Length;

            var decoder = Encoding.UTF8.GetDecoder();
            var totalLength = 0;
            var processedChars = 0;
            Span<char> charSpan = stackalloc char[estimate];

            foreach (var memoryByte in buffer)
            {
                totalLength += memoryByte.Length;
                var isLast = totalLength == buffer.Length;
                var emptyCharSlice = charSpan[processedChars..];
                var charCount = decoder.GetChars(memoryByte.Span, emptyCharSlice, isLast);
                processedChars += charCount;
            }

            var finalCharacters = charSpan.Slice(0, processedChars);
            buffer = buffer.Slice(processedChars);
            return new string(finalCharacters);
        }
    }
}
