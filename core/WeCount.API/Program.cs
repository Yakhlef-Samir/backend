using WeCount.Bootstrap;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }
        builder
            .Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{builder.Environment.EnvironmentName}.json",
                optional: true,
                reloadOnChange: true
            )
            .AddEnvironmentVariables();

        // Configure les services via Bootstrap
        builder.Services.AddBootstrapServices(builder.Configuration);

        var app = builder.Build();

        // Configure le pipeline via Bootstrap
        app.ConfigureBootstrapPipeline();

        app.Run();
    }
}
