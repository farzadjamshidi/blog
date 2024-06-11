
using Blog.API.Setup.MongoDb;
using Serilog;
using Serilog.Events;

namespace Blog.API.Setup;

internal static class SerilogSetup
{
    public static void AddSerilog(LogSetupConfig config)
    {
        var setup = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.With<RemoveExtraPropertiesEnricher>()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error);

        if (config.EnabledConsoleOutput)
        {
            setup = setup.AddConsoleLogging();
        }

        setup = config.DatabaseType switch
        {
            DatabaseType.None => setup,
            DatabaseType.MongoDB => setup.AddMongoDb(config),
            _ => throw new NotImplementedException(),
        };

        Log.Logger = setup.CreateLogger();
    }

    private static LoggerConfiguration AddConsoleLogging(this LoggerConfiguration setup)
    {
        Serilog.Debugging.SelfLog.Enable(Console.WriteLine);
        return setup.WriteTo.Console(LogEventLevel.Information);
    }
}