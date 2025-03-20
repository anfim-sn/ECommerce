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

var context = app.Services.GetRequiredService<DapperDbContext>();
context.DbConnection.ExecuteAsync("IF NOT EXISTS (SELECT 1 FROM pg_tables WHERE tablename = 'Users')    \n    CREATE TABLE public.'Users'\n    (\n        UserId     UUID PRIMARY KEY NOT NULL DEFAULT uuid_generate_v4(),\n        Email      VARCHAR(255) UNIQUE NOT NULL,\n        Password   TEXT                NOT NULL,\n        PersonName VARCHAR(255),\n        Gender     VARCHAR(50)\n    );");

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