using BedrockRtmp.Messages;
using BedrockRtmp.Messages.Handshake;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BedrockRtmp
{
    internal static class RtmpHandshake
    {
        public static async Task ReadHandshakeAsync(RtmpMessageHandler messageHandler)
        {
            var c0 = await messageHandler.ReadAsync<Handshake0Message>();
            var c1 = await messageHandler.ReadAsync<Handshake1Message>();
            // todo: figure out why OBS is saying the handshake is failing now
            // like why wasn't it failing when we were doing nothing but it does now that we're reading the handshake
            Debugger.Break();
        }
    }
}
