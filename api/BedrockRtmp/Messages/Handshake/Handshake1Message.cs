using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace BedrockRtmp.Messages.Handshake
{
    public class Handshake1Message : RtmpMessage
    {
        public uint Timestamp { get; private set; }
        public uint Zero { get; private set; }
        public byte[] RandomBytes { get; private set; } = new byte[1528];

        public override int CalculateSize() => 4 + 4 + 1528;

        public override void Read(SequenceReader<byte> reader, ref SequencePosition consumed, ref SequencePosition examined)
        {
            Timestamp = BinaryPrimitives.ReadUInt32BigEndian(reader.UnreadSpan);
            reader.Advance(4);
            Zero = BinaryPrimitives.ReadUInt32BigEndian(reader.UnreadSpan);
            reader.Advance(4);

            for (int i = 0; i < RandomBytes.Length; i++)
            {
                if (reader.TryRead(out var randomByte))
                {
                    RandomBytes[i] = randomByte;
                }
            }

            consumed = reader.Position;
            examined = consumed;
        }

        public override void Write(ref Span<byte> output)
        {
            throw new NotImplementedException();
        }
    }
}
