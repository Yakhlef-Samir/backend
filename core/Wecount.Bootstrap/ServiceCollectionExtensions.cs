using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using WeCount.Application.Users.Commands;
using WeCount.Infrastructure.Interfaces;
using WeCount.Infrastructure.MongoDB;
using WeCount.Infrastructure.Repositories.UserRepository;

namespace WeCount.Bootstrap
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBootstrapServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            //** Web API services
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeCount API", Version = "v1" });
            });

            //** MongoDB settings
            services.Configure<MongoDbSettings>(config.GetSection("MongoDbSettings"));
            services.AddSingleton<IMongoClient>(sp => new MongoClient(
                config["MongoDbSettings:ConnectionString"]
            ));

            //** Context
            services.AddScoped<IMongoDbContext, MongoDbContext>();

            //** Repository
            services.AddScoped<IUserRepository, UserRepository>();

            //** MediatR handlers
            services.AddMediatR(typeof(CreateUserCommand).Assembly);

            return services;
        }

        public static WebApplication ConfigureBootstrapPipeline(this WebApplication app)
        {
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

            return app;
        }
    }
}
