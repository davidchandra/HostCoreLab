namespace HostCoreDocker
{
    using System;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using System.IO;

    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            //Setup and configure SeriLog to write to console
            Log.Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .Enrich.WithThreadId()
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}][{ThreadId}] {Message:lj} {Properties}{NewLine}{Exception}")
                            .CreateLogger();

            try
            {
                 //Create and run the generic host 
                await CreateHostBuilder(args)
                         .Build()
                         .RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
                Console.ReadKey(); // so the console window does not immediately close
            }
        }


        //Setup the host configurations
        static IHostBuilder CreateHostBuilder(string[] args) => new HostBuilder()
                .ConfigureHostConfiguration(hostConfig =>
                {
                    hostConfig.SetBasePath(Directory.GetCurrentDirectory());
                    hostConfig.AddCommandLine(args);
                    hostConfig.AddEnvironmentVariables("HOSTCORE_");
                })
                .ConfigureAppConfiguration((hostContext, appConfig) =>
                {
                    appConfig.AddJsonFile("myconfig.json", optional: false, reloadOnChange: true);
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<WorkerHostedService>();
                    services.AddLogging();
                })
                .UseSerilog()
                .UseConsoleLifetime();
    }
}
