using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.Posts;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPostRepository, PostRepository>();

            return services;
        }
    }
}
