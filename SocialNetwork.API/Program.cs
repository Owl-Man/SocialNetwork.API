using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

builder.Services.AddDbContext<SocialNetworkDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(SocialNetworkDbContext)));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpsRedirection();

app.UseAuthentication();

app.Run();