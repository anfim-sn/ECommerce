using ECommerce.Api.Middlewares;
using ECommerce.Core;
using ECommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure();
builder.Services.AddCore();

// Add controllers.
builder.Services.AddControllers();

// Build the app.
var app = builder.Build();

// Add middlewares
app.UseExceptionHandlingMiddleware();

//Routing
app.UseRouting();

//Auth
app.UseAuthentication();
app.UseAuthorization();

//Controller routes
app.MapControllers();

app.MapGet("/", () => "Hello World!");
app.Run();