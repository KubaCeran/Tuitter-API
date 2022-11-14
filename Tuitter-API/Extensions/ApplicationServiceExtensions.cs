using Tuitter_API.Service;
using Tuitter_API.Service.User;

namespace Tuitter_API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<ILoggedUserService, LoggedUserService>();

            return services;
        }
    }
}
