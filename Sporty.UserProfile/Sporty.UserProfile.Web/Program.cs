using System.Reflection;
using System.Text.Json.Serialization;
using Serilog;
using Sporty.UserProfile.Core;
using Sporty.UserProfile.Data;
using Sporty.UserProfile.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"))
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services
    .AddControllers(options => options.Filters.Add<ExceptionFilter>())
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var configuration = builder.Configuration;

builder.Services.AddCore();
builder.Services.AddData(configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(opt => opt.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
app.UseAuthentication();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();