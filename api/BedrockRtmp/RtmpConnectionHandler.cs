using BedrockRtmp.Extensions;
using BedrockRtmp.Messages;
using BedrockRtmp.Messages.Handshake;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Threading.Tasks;

namespace BedrockRtmp
{
    public class RtmpConnectionHandler : ConnectionHandler
    {
        private readonly ILogger<RtmpConnectionHandler> _logger;

        public RtmpConnectionHandler(ILogger<RtmpConnectionHandler> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            var messageHandler = new RtmpMessageHandler(connection.Transport);

            try
            {
                _logger.LogInformation("{Id} connected.", connection.ConnectionId);
                await PerformHandshakeAsync(messageHandler);

                while (connection.Transport.Input.TryRead(out var result))
                {
                    var buffer = result.Buffer;
                    // todo: handle messages
                    connection.Transport.Input.AdvanceTo(buffer.Start, buffer.End);
                    if (result.IsCompleted)
                    {
                        break;
                    }
                }
            }
            finally
            {
                _logger.LogInformation("{Id} disconnected.", connection.ConnectionId);
            }
        }

        private async Task PerformHandshakeAsync(RtmpMessageHandler messageHandler)
        {
            // ok so the following DEFINITELY does not work:
            // foreach (var @byte in c0AndC1Buffer)
            // {
            //     await connection.Transport.Output.WriteAsync(@byte, connection.ConnectionClosed);
            // }
            // that makes obs invalidate the handshake (or I guess we invalidate obs's handshake)

            await RtmpHandshake.ReadHandshakeAsync(messageHandler);
            // todo: add the rest of the return message
        }
    }
}
