using DB.Contexts;
using Microsoft.EntityFrameworkCore;

namespace API_Pasteleria.Extensions
{
    public static class DatabaseExtension
    {
        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            var services = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = services.ServiceProvider.GetService<ApplicationDbContext>();
            dbContext?.Database.Migrate();
        }
    }
}
