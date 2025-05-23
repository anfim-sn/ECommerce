using System.Text.Json.Serialization;
using Dapper;
using ECommerce.Api.Middlewares;
using ECommerce.Core;
using ECommerce.Core.Mappers;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.dbcontext;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure();
builder.Services.AddCore();

// Add controllers.
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

//Take All profiles from Assembly inherited from Profile interface
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

//Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();

//Add Api Explorer
builder.Services.AddEndpointsApiExplorer();

//Add swagger generation services to create swagger specification
builder.Services.AddSwaggerGen();

//Add Cors
builder.Services.AddCors(o => 
    o.AddDefaultPolicy(b => 
        b.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
        )
    );

// Build the app.
var app = builder.Build();

await ApplyMigrations(app.Services);

// Add middlewares
app.UseExceptionHandlingMiddleware();

//Routing
app.UseRouting();

//Add endpoints for swagger.json
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

//Auth
app.UseAuthentication();
app.UseAuthorization();

//Controller routes
app.MapControllers();

app.Run();

async Task ApplyMigrations(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DapperDbContext>();

    context.DbConnection.Open();
    var a = await context.DbConnection.ExecuteAsync("CREATE TABLE IF NOT EXISTS \"users\"\n    (\n        \"UserId\"     UUID PRIMARY KEY NOT NULL,\n        \"Email\"      VARCHAR(255) UNIQUE NOT NULL,\n        \"Password\"   TEXT                NOT NULL,\n        \"PersonName\" VARCHAR(255),\n        \"Gender\"     VARCHAR(50)\n    );");
    var b = await context.DbConnection.ExecuteAsync("INSERT INTO users (\"UserId\", \"Email\", \"PersonName\", \"Gender\", \"Password\")\nVALUES ('c32f8b42-60e6-4c02-90a7-9143ab37189f', 'test1@example.com', 'John Doe', 'Male', 'password1'),\n       ('8ff22c7d-18c7-4ef0-a0ac-988ecb2ac7f5', 'test2@example.com', 'Jane Smith', 'Female', 'password2') \nON CONFLICT DO NOTHING;\n");
    context.DbConnection.Close();
}