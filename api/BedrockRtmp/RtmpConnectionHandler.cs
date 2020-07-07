using BedrockRtmp.Extensions;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
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
            // todo: implement actual handler
            try
            {
                _logger.LogInformation("{Id} connected.", connection.ConnectionId);

                while (true)
                {
                    var result = await connection.Transport.Input.ReadAsync();
                    var buffer = result.Buffer;
                    _logger.LogInformation("Message: {Message}", buffer.ParseAsUTF8String());

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
    }
}
