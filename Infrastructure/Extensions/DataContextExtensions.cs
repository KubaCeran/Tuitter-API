using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class DataContextExtensions
    {
        public static IServiceCollection RegisterDataContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<TuitterContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
