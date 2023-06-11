using Assessment.Core.DbContexts;
using Assessment.Core.Domain.Models;
using Assessment.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", false, true)
               .AddJsonFile($"appsettings.{env}.json", true)
               .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();


try
{
    Log.Logger.Information("Starting up...");

    // Add services to the container.
    var services = builder.Services;

    services.AddControllers();
    services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddSerilog();

    services.AddDbContext<AssessmentContext>(options =>
        options.UseSqlServer(config.GetSection("ConnectionStrings:AssessmentDb").Value));

    services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
    services.AddTransient(typeof(IRepository<Order>), typeof(Repository<Order>));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    await app.RunAsync();

}

catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Application start-up failed");
    throw;
}

finally
{
    Log.CloseAndFlush();
}
