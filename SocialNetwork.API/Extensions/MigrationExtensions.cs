using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess;

namespace SocialNetwork.API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        try
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

            using SocialNetworkDbContext context = serviceScope.ServiceProvider.GetService<SocialNetworkDbContext>();

            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при применении миграций: {ex.Message}");
            throw;
        }
    }
}
