using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.IO;

namespace cancel
{
    public class TimedHostedService: IHostedService, IDisposable
    {
         private Timer _timer;
         private readonly ILogger _logger;

         public TimedHostedService(ILogger<TimedHostedService> logger)
         {
             _logger = logger;
         }

         public Task StartAsync(CancellationToken cancellationToken)
         {
             _logger.LogInformation("Timed background service is starting");

             _timer = new Timer(WriteLogFile, null,TimeSpan.Zero, TimeSpan.FromSeconds(10));
             
             return Task.CompletedTask;
         }
         public Task StopAsync(CancellationToken cancellationToken)
         {
             _logger.LogInformation("Timed background service is stoping");

             _timer?.Change(Timeout.Infinite, 0);
             
             return Task.CompletedTask;
         }


         private void WriteLogFile(object state)
         {
             using(var streamWriter = new StreamWriter(@"data.txt"))
             {
                 streamWriter.WriteLine(DateTime.Now.ToString());
             }
         }

         public void Dispose()
         {
             _timer?.Dispose();
         }
    }
}