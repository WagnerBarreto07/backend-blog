using API.Config;
using API.Middlewares;
using Domain.DTO.Mappings;
using Infra.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.WebSockets;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer'[space] and then your token in the text input below. \r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                      new OpenApiSecurityScheme
                      {
                          Reference = new OpenApiReference
                          {
                              Type = ReferenceType.SecurityScheme,
                              Id = "Bearer"
                          }
                      },
                     new string[] {}
                }
            });
});

var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configManager = new ConfigurationBuilder()
.AddJsonFile($"appsettings.{enviroment}.json", true, true)
.AddEnvironmentVariables()
.Build();

var key = Encoding.ASCII.GetBytes(configManager["Secret"]);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

ServicesConfig config = new ServicesConfig(builder.Services);

var serverVersion = new MySqlServerVersion(new Version(8, 0, 2));

builder.Services.AddDbContext<BlogContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion,
    x => x.EnableRetryOnFailure(
        maxRetryCount: 3,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null));
});

var app = builder.Build();

app.UseWebSockets();

app.MapGet("/", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    else
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

        var data = Encoding.ASCII.GetBytes("Nova Postagem Publicada!");

        await webSocket.SendAsync(data, WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);    
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(builder =>
{
    builder
       .WithOrigins("http://localhost:4200", "https://localhost:4200")
       .SetIsOriginAllowedToAllowWildcardSubdomains()
       .AllowAnyHeader()
       .AllowCredentials()
       .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
       .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
});

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware(typeof(ErrorMiddleware));

app.MapControllers();

await app.RunAsync();
