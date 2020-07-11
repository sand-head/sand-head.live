using System;
using System.Buffers;

namespace BedrockRtmp.Messages
{
    public abstract class RtmpMessage
    {
        /// <summary>
        /// Calculates the number of bytes used by the given message.
        /// </summary>
        public abstract int CalculateSize();
        public abstract void Read(SequenceReader<byte> reader, ref SequencePosition consumed, ref SequencePosition examined);
        public abstract void Write(ref Span<byte> output);
    }
}
