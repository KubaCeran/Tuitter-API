using Tuitter_API.Repository.Categories;
using Tuitter_API.Repository.Post;

namespace Tuitter_API.Extensions
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
