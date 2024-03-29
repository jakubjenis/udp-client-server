using System.Net;

namespace UdpServer;

public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                const int port = 3333;

                var serverLogger = serviceProvider.GetService<ILogger<MyUdpServer>>()!;
                var server = new MyUdpServer(serverLogger, IPAddress.Any, port);

                // Start the server
                logger.LogDebug("Server starting...");
                server.Start();
                logger.LogInformation("Server started on port {Port}", port);
                logger.LogInformation($"Press q to exit");
                
                while(true)
                {
                    var line = Console.ReadLine();
                    if (line == "q") break;
                }

                // Stop the server
                logger.LogDebug("Server stopping...");
                server.Stop();
                logger.LogInformation("Server stopped");
            }
        }
        catch (OperationCanceledException)
        {
            // When the stopping token is canceled, for example, a call made from services.msc,
            // we shouldn't exit with a non-zero exit code. In other words, this is expected...
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);

            // Terminates this process and returns an exit code to the operating system.
            // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
            // performs one of two scenarios:
            // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
            // 2. When set to "StopHost": will cleanly stop the host, and log errors.
            //
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            Environment.Exit(1);
        }

        return Task.CompletedTask;
    }   
}