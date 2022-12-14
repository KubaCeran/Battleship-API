using Battleship_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Battleship_API.Extensions
{
    public static class DataContextExtension
    {
        public static IServiceCollection RegisterDataContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
