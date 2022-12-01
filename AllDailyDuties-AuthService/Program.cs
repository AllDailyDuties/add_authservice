using AllDailyDuties_AuthService.Helpers;
using AllDailyDuties_AuthService.Middleware.Authorization;
using AllDailyDuties_AuthService.Middleware.Authorization.Interfaces;
using AllDailyDuties_AuthService.Middleware.Messaging;
using AllDailyDuties_AuthService.Middleware.Messaging.Interfaces;
using AllDailyDuties_AuthService.Services;
using AllDailyDuties_AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//TODO: Set this up to only accept the forwarded headers from the load balancer
var factory = new ConnectionFactory
{
    HostName = "localhost"
};
//Create the RabbitMQ connection using connection factory details as i mentioned above
var connection = factory.CreateConnection();
//Here we create channel with session and model
using var channel = connection.CreateModel();

// Add services to the container.
builder.Services.AddDbContext<DataContext>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
builder.Services.AddScoped<IRabbitMQConsumer, RabbitMQConsumer>();

//builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = TokenService.CreateTokenValidationParameters();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var rabbitMQ = scope.ServiceProvider.GetRequiredService<IRabbitMQConsumer>();
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.EnsureCreated();
    rabbitMQ.ConsumeMessage(channel, "auth_token");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var forwardOpts = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
};

//Console.ReadKey();
forwardOpts.KnownNetworks.Clear();
forwardOpts.KnownProxies.Clear();
app.UseForwardedHeaders(forwardOpts);

app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

app.MapGet("/", test);
async Task test(HttpContext context)
{
    byte[] bytes = Encoding.ASCII.GetBytes("foo");
    await context.Response.Body.WriteAsync(bytes);
}
app.Run("http://+:9000");

