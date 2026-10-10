using System.Reflection;
using System.Text.Json.Serialization;
using LocadoraVeiculos.Api.Middleware;
using LocadoraVeiculos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LocadoraDbContext>(options =>
{
    if (connectionString != null && connectionString.StartsWith("Data Source="))
        options.UseSqlite(connectionString);
    else
        options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LocadoraVeiculos.Infrastructure"));
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Locadora de Veículos API",
        Version = "v1",
        Description = "API RESTful para gerenciamento de locadora de veículos desenvolvida com ASP.NET Core e Entity Framework Core."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

var conn = app.Configuration.GetConnectionString("DefaultConnection");
if (conn != null && conn.StartsWith("Data Source="))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<LocadoraDbContext>();
    db.Database.EnsureCreated();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Locadora de Veículos API v1");
});

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
