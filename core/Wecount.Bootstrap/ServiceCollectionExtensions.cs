using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using WeCount.Application.Common.Interfaces;
using WeCount.Application.Common.Mapping;
using WeCount.Application.Users.Commands;
using WeCount.Domain.Entities;
using WeCount.Domain.Entities.Budget;
using WeCount.Domain.Entities.Couple;
using WeCount.Domain.Entities.Transaction;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Infrastructure.MongoDB;
using WeCount.Infrastructure.Repositories.BudgetRepository;
using WeCount.Infrastructure.Repositories.CoupleRepository;
using WeCount.Infrastructure.Repositories.DebtRepository;
using WeCount.Infrastructure.Repositories.GoalRepository;
using WeCount.Infrastructure.Repositories.TransactionCategoryRepository;
using WeCount.Infrastructure.Repositories.TransactionRepository;
using WeCount.Infrastructure.Repositories.UserRepository;

using WeCount.Infrastructure.Services;

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
            services.AddScoped<ICoupleRepository, CoupleRepository>();
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<IDebtRepository, DebtRepository>();
            services.AddScoped<IGoalRepository, GoalRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionCategoryRepository, TransactionCategoryRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, TokenService>();
            // Collections MongoDB pour handlers
            services.AddScoped<IMongoCollection<User>>(sp =>
                sp.GetRequiredService<IMongoDbContext>().Users
            );
            services.AddScoped<IMongoCollection<Couple>>(sp =>
                sp.GetRequiredService<IMongoDbContext>().Couples
            );
            services.AddScoped<IMongoCollection<Transaction>>(sp =>
                sp.GetRequiredService<IMongoDbContext>().Transactions
            );
            services.AddScoped<IMongoCollection<TransactionCategory>>(sp =>
                sp.GetRequiredService<IMongoDbContext>().TransactionCategories
            );
            services.AddScoped<IMongoCollection<Budget>>(sp =>
                sp.GetRequiredService<IMongoDbContext>().Budgets
            );
            services.AddScoped<IMongoCollection<Goal>>(sp =>
                sp.GetRequiredService<IMongoDbContext>().Goals
            );
            services.AddScoped<IMongoCollection<Debt>>(sp =>
                sp.GetRequiredService<IMongoDbContext>().Debts
            );

            // AutoMapper & MappingService
            services.AddAutoMapper(typeof(MapperService).Assembly);
            services.AddScoped<IMapperService, MapperService>();

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
