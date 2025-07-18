using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using WeCount.Application.Users.Commands;
using WeCount.Infrastructure.Interfaces;
using WeCount.Infrastructure.MongoDB;
using WeCount.Infrastructure.Repositories.UserRepository;

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

        // Ajoute les services nécessaires
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeCount API", Version = "v1" });
        });

        // Configure MongoDB et Controllers
        builder.Services.AddControllers();
        // Enregistre MediatR
        builder.Services.AddMediatR(typeof(CreateUserCommand).Assembly);
        builder.Services.AddOptions();
        builder.Services.Configure<MongoDbSettings>(
            builder.Configuration.GetSection("MongoDbSettings")
        );
        builder.Services.AddSingleton<IMongoClient>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(opts.ConnectionString);
        });
        builder.Services.AddScoped(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return sp.GetRequiredService<IMongoClient>().GetDatabase(opts.DatabaseName);
        });

        // Enregistre MongoDbContext et UserRepository
        builder.Services.AddScoped<IMongoDbContext, MongoDbContext>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        var app = builder.Build();

        // Configure le pipeline HTTP
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeCount API V1");
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
