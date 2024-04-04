using Core.Entities;

namespace Infrastructure.Repositories.Categories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> FindCategoryByName(IEnumerable<string> categories);
    }
}
