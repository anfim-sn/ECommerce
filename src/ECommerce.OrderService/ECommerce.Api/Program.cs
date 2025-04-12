using BusinessLogicLayer;
using BusinessLogicLayer.HttpClients;
using BusinessLogicLayer.Policies;
using DataAccessLayer;
using ECommerce.Api.Middleware;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ECommerce.OrderService", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddTransient<IBasePolicies, BasePolicies>();
builder.Services.AddTransient<IUsersMicroservicePolicy, UsersMicroservicePolicy>();
builder.Services.AddTransient<IProductsMicroservicePolicy, ProductsMicroservicePolicy>();

builder.Services
    .AddHttpClient<UsersMicroserviceClient>(client => {
        client.BaseAddress = new Uri(
            $"http://{builder.Configuration["UsersMicroserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}");
    })
    .AddPolicyHandler(builder.Services.BuildServiceProvider().GetRequiredService<IUsersMicroservicePolicy>().GetCombinedPolicy());

builder.Services
    .AddHttpClient<ProductsMicroserviceClient>(client => {
    client.BaseAddress = new Uri(
        $"http://{builder.Configuration["ProductsMicroserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}"); 
    })
    .AddPolicyHandler(builder.Services.BuildServiceProvider().GetRequiredService<IProductsMicroservicePolicy>().GetCombinedPolicy());

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();