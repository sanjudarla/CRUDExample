using CRUDExample;
using CRUDExample.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Logging

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{

    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(service);
});
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}
app.UseSerilogRequestLogging();
app.UseHttpLogging();


if (builder.Environment.IsEnvironment("Test") == false)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();


public partial class Program { }