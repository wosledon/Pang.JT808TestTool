using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Pang.JT808TestTool.Session;
using SuperSocket;
using SuperSocket.ProtoBase;
using SuperSocket.SessionContainer;

namespace Pang.JT808TestTool.ServerService
{
    public class TcpServerService:BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var hosting = SuperSocketHostBuilder.Create<TextPackageInfo>()
                .UseClearIdleSession()
                .ConfigureSuperSocket(opts =>
                {
                    opts.AddListener(new ListenOptions()
                    {
                        Ip = "Any",
                        Port = 10808
                    });
                })
                .UseSession<JTT808TcpSession>()
                .UseSessionHandler(onConnected: async (s) =>
                {
                    await Task.CompletedTask;
                }, onClosed: async (s, v) =>
                {
                    await Task.CompletedTask;
                })
                .UsePackageHandler(async (s, p) =>
                {
                    await Task.CompletedTask;
                })
                .UseInProcSessionContainer()
                .UseMiddleware<InProcSessionContainerMiddleware>()
                .BuildAsServer();

            hosting.StartAsync();
            return Task.CompletedTask;
        }
    }
}