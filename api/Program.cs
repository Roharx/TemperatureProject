using System.Text;
using api.Config;
using api.Helpers;
using api.Middleware;
using api.Services;
using api.Interfaces;
using infrastructure;
using infrastructure.Interfaces;
using infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using service.Interfaces;
using service.Services;
using WebSocketServer = api.Websockets.WebSocketServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<INpgsqlConnectionFactory>(sp => new NpgsqlConnectionFactory(Utilities.ProperlyFormattedConnectionString));
builder.Services.AddSingleton<IDatabase, DapperDatabase>();
builder.Services.AddSingleton<ICrudHandler, CrudHandler>();
builder.Services.AddSingleton<ICrudService, CrudService>();
builder.Services.AddSingleton<IHashService, HashService>();
builder.Services.AddSingleton<IActionLogger, ActionLogger>();
builder.Services.AddSingleton<IMqttService, MqttService>();

var webSocketServerUrl = "ws://0.0.0.0:8181";
Console.WriteLine("Starting ws server at: " + webSocketServerUrl);
builder.Services.AddSingleton<api.Interfaces.IWebSocketServer, WebSocketServer>(sp =>
{
    var webSocketServer = new WebSocketServer(webSocketServerUrl);
    webSocketServer.Start(socket => { });
    return webSocketServer;
});

// Helpers
builder.Services.AddSingleton<RequestHandler>();

// Bind JWT settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Register TokenService with configured settings
builder.Services.AddSingleton<ITokenService, TokenService>(sp =>
{
    var jwtSettings = sp.GetRequiredService<IOptions<JwtSettings>>().Value;
    return new TokenService(jwtSettings);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://161.97.92.174", "http://161.97.92.158", "http://localhost:4200") // Update this to your frontend's URL
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Bind MQTT settings
builder.Services.Configure<MqttSettings>(builder.Configuration.GetSection("MqttSettings"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
}

if (builder.Environment.IsProduction())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString);
}

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtConfig = builder.Configuration.GetSection("Jwt");
    var key = Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("Key")
                                     ?? throw new NullReferenceException("JWT key cannot be null"));
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new[] { "" }
        }
    });
});

builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares
app.UseMiddleware<GlobalExceptionHandler>();
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication(); // Ensure Authentication middleware is added
app.UseAuthorization();

app.UseWebSockets(); // Add this line to enable WebSocket support
// app.UseMiddleware<WebSocketMiddleware>(); // Comment out or remove this line if not needed

app.MapControllers();

// Start the MQTT service
var mqttService = app.Services.GetRequiredService<IMqttService>();
await mqttService.StartAsync();

app.Run();
