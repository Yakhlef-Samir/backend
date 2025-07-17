using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Ajoute les services nécessaires
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeCount API", Version = "v1" });
});

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
app.Run();