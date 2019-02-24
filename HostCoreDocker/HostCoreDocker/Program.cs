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
            try
            {
                //Create the generic host and setup configurations
                var host = CreateHostBuilder(args)
                         .Build();

                var config = host.Services.GetService<IConfiguration>();
                
                var sectionExists = config.GetSection("seqSettings").Exists();

                //Load configuration so it can be used as part of this setup
                MyConfiguration myconfig = new MyConfiguration();
                if (sectionExists)
                    config.GetSection("seqSettings").Bind(myconfig);
                else
                    throw new ConfigurationException()
                    {
                        ConfigurationKey = "seqSetting",
                        ConfigurationDescription = "URL pointing to SEQ Server for logging is not specified or found in the configuration file."
                    };

                //Setup and configure SeriLog to write to console
                var logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .Enrich.WithThreadId()
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}][{ThreadId}] {Message:lj} {Properties}{NewLine}{Exception}")
                            .CreateLogger();

                Log.Logger = logger;

                await host.RunAsync();
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
                })
                .UseSerilog()
                .UseConsoleLifetime();
    }
}
