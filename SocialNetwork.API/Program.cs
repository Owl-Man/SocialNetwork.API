using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Services;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.DataAccess;
using SocialNetwork.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

var configuration = builder.Configuration;

builder.Services.AddDbContext<SocialNetworkDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(SocialNetworkDbContext)));
    });

builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<IUsersRepository, CachedUserRepository>();

builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    redisOptions.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();