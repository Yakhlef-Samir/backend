using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WeCount.Infrastructure.MongoDB;

namespace WeCount.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOptions();
            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            services.AddSingleton<IMongoClient>(sp =>
            {
                var opts = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(opts.ConnectionString);
            });
            services.AddScoped(sp =>
            {
                var opts = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return sp.GetRequiredService<IMongoClient>().GetDatabase(opts.DatabaseName);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
