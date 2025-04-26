using Microsoft.AspNetCore.Server.Kestrel.Core;
using StackExchange.Redis;
using succotashBE.UserManagement;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using succotashBE.GQL.QueryResolver;
using succotashBE.GQL.MutationQueryResolver;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// adding the graphql endpoints (mutaions and query resolvers)

builder.Services.AddGraphQLServer()
    .AddQueryType(q => q.Name("Query"))
    .AddType<UserManagement>();
    //.AddMutationType(m => m.Name("Mutation"))
    //.AddType<ClientMutationResolver>()
//;

builder.Services.AddServices();
builder.Services.AddGraphQLServer();


// JwtBearer bearer aunthentication

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

var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;
var options = ConfigurationOptions.Parse(redisConnectionString);
options.AbortOnConnectFail = false;
options.Ssl = false;

var redisConnection = ConnectionMultiplexer.Connect(options);
builder.Services.AddSingleton<IConnectionMultiplexer>(redisConnection); // <-- Register as Singleton



var app = builder.Build();

// when under development use swagger

if (app.Environment.IsDevelopment())
{
    app.MapGraphQL();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowSpecificOrigins");
}
else
{
    app.MapGraphQL();
    app.UseCors("ProdAllowSpecificOrigins");
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();