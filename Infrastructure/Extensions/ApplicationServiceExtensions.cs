using Infrastructure.Services.Posts;
using Infrastructure.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<ILoggedUserService, LoggedUserService>();
            services.AddScoped<IPostService, PostService>();


            return services;
        }
    }
}
