using LocadoraVeiculos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LocadoraDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LocadoraVeiculos.Infrastructure")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Locadora de Veículos API",
        Version = "v1",
        Description = "API para sistema de locadora de veículos"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Locadora de Veículos API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    Sistema = "Locadora de Veículos API",
    Status = "Online",
    Swagger = "/swagger"
}));

app.Run();
