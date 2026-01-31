using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Hike
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logDir = Path.Combine(Environment.CurrentDirectory, "logs");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            var logPath = Path.Combine(logDir, "log_file.txt");
            if (!File.Exists(logPath))
                File.Create(logPath).Dispose();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{SourceContext} {Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    new RenderedCompactJsonFormatter(),
                    logPath,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10L * 1024 * 1024,
                    flushToDiskInterval: TimeSpan.FromSeconds(2))
                .CreateLogger();
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
                CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR ON RUN!!!");
                Console.WriteLine(e);
                Log.Fatal(e, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
