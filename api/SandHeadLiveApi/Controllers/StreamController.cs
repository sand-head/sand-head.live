using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SandHeadLiveApi.Hubs;
using Serilog;
using System.Threading.Tasks;

namespace SandHeadLiveApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class StreamController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public StreamController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("start"), Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> StartStreamAsync()
        {
            // NGINX RTMP sends a few arguments through a form
            // "name" is the stream name/key
            var name = Request.Form["name"];
            if (string.IsNullOrWhiteSpace(name)) return BadRequest();

            // todo: prevent random streaming using stream key

            await _hubContext.Clients.All.SendAsync("messageReceived", "Server", "0000", "Justin is going live!");
            Log.Information("Stream starting");

            // todo: Redirect here instead of Ok
            return Ok();
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Hello World!");
        }
    }
}
