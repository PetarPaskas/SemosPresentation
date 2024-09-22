using DocumentService.BackgroundFileProcessing.Processes;
using Microsoft.Extensions.Hosting;


namespace DocumentService.BackgroundFileProcessing;

public class HostProgram : IHostedService
{
    private IFileGenerationProcess _fileGenerationProcess;
    public HostProgram(FileGenerationProcess fileGenerationProcess)
    {
        _fileGenerationProcess = fileGenerationProcess; 
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {

        while(!cancellationToken.IsCancellationRequested)
        {
           Task.Factory.StartNew(_fileGenerationProcess.Process, TaskCreationOptions.AttachedToParent);

            Thread.Sleep(2000);
        }

        if (cancellationToken.IsCancellationRequested)
           await StopAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}
