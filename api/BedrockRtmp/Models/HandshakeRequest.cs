using System;
using System.Collections.Generic;
using System.Text;

namespace BedrockRtmp.Models
{
    internal class HandshakeRequest
    {
        public byte Version { get; set; }
        public uint Timestamp { get; set; }
    }
}
