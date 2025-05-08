using Serilog;
using SerilogStructuredLogging.Services;

// Configure the global logger instance
Log.Logger = new LoggerConfiguration()
    // Set the logger to output logs to the console window
    .WriteTo.Console()
    // Finalize and create the logger instance
    .CreateLogger();

try
{
    Log.Information("starting server");

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    //Register the serilog logger service in DI Container
    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        // Write logs to the console (standard output)
        loggerConfiguration.WriteTo.Console();

        // Read additional Serilog settings (like minimum level, sinks, etc.) from appsettings.json or appsettings.{Environment}.json
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    //Register the DummyService
    builder.Services.AddTransient<IDummyService, DummyService>();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    // Enable Serilog's built-in middleware to automatically log HTTP requests, including method, path, status code, and execution time
    app.UseSerilogRequestLogging();

    app.MapGet("/", (IDummyService svc) => svc.DoSomething());

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
