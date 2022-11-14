using Microsoft.EntityFrameworkCore;
using Tuitter_API.Data.DataContext;

namespace Tuitter_API.Extensions
{
    public static class DataContextExtensions
    {
        public static IServiceCollection RegisterDataContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
