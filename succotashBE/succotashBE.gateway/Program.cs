using Microsoft.AspNetCore.Server.Kestrel.Core;
using StackExchange.Redis;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policies
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Adjust as needed
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
    options.AddPolicy("ProdAllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("https://your-production-url.com")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add GraphQL (HotChocolate)
builder.Services.AddGraphQLServer();

// JWT Bearer authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
        };
    });

// Redis configuration
var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;
var options = ConfigurationOptions.Parse(redisConnectionString);
options.AbortOnConnectFail = false;
options.Ssl = false;
var redisConnection = ConnectionMultiplexer.Connect(options);
builder.Services.AddSingleton<IConnectionMultiplexer>(redisConnection);

// Add Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true); // Production Gateway

builder.Services.AddOcelot();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowSpecificOrigins");
    app.MapGraphQL();
}
else
{
    app.UseCors("ProdAllowSpecificOrigins");
    app.MapGraphQL();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseOcelot().Wait();
app.MapControllers();

app.Run();