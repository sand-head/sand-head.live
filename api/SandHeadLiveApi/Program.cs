using BedrockRtmp;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SandHeadLiveApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel(options =>
                    {
                        // RTMP over port 1935
                        options.ListenLocalhost(1935, builder =>
                            builder.UseConnectionLogging().UseConnectionHandler<RtmpConnectionHandler>());

                        // HTTP over port 5000
                        options.ListenLocalhost(5000);
                    });
                });
    }
}
