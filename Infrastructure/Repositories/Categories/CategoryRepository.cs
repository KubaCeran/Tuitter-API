using Core.Entities;
using Infrastructure.DataContext;

namespace Infrastructure.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task<Category> FindCategoryByName(string categoryName);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TuitterContext _context;

        public CategoryRepository(TuitterContext context)
        {
            _context = context;
        }

        public async Task<Category> FindCategoryByName(string categoryName)
        {
            return _context.Categories.SingleOrDefault(x => x.Title == categoryName);
        }
    }
}
