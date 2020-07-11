using System;
using System.Buffers;

namespace BedrockRtmp.Messages.Handshake
{
    public class Handshake0Message : RtmpMessage
    {
        public byte Version { get; private set; }

        public override int CalculateSize() => sizeof(byte);

        public override void Read(SequenceReader<byte> reader, ref SequencePosition consumed, ref SequencePosition examined)
        {
            if (!reader.TryRead(out var version) || version > 31)
            {
                throw new Exception("Could not read C0/S0 from handshake, or version value was invalid (must be between 0 and 31).");
            }

            Version = version;
            consumed = reader.Position;
            examined = consumed;
        }

        public override void Write(ref Span<byte> output)
        {
            output[0] = Version;
        }
    }
}
