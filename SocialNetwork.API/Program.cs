using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SocialNetwork.API.Extensions;
using SocialNetwork.Application.Services;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.DataAccess;
using SocialNetwork.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SocialNetwork API", Version = "v1" });
});

builder.Services.AddOpenApi();

var configuration = builder.Configuration;

builder.Services.AddDbContext<SocialNetworkDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(SocialNetworkDbContext)));
    });

builder.Services.AddScoped<IPostsService, PostsService>();

builder.Services.AddScoped<PostsRepository>();
builder.Services.AddScoped<IPostsRepository, CachedPostsRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<IUsersRepository, CachedUsersRepository>();

builder.Services.AddScoped<IFeedService, FeedService>();

builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    redisOptions.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

//FOR TESTING ALLOWED ALL IN CORS
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
    context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
    context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization");

    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 204;
        return;
    }

    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("http://localhost:5000/openapi/v1.json", "SocialNetwork API V1");
        c.RoutePrefix = "swagger";
    });

    app.ApplyMigrations();
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.UseRouting();

app.Run();