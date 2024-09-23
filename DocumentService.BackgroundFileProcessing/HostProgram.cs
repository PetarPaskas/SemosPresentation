using DocumentService.BackgroundFileProcessing.Processes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocumentService.BackgroundFileProcessing;

public class HostProgram : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HostProgram(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("HOST PROGRAM START");
        while (!cancellationToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var fileGenerationProcess = scope.ServiceProvider.GetRequiredService<IFileGenerationProcess>();
                await fileGenerationProcess.Process();
            }

            await Task.Delay(2000, cancellationToken); 
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("HOST PROGRAM END");
    }
}
