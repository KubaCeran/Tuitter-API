using Tuitter_API.Data.DataContext;
using Tuitter_API.Data.Entities;

namespace Tuitter_API.Repository.Categories
{
    public interface ICategoryRepository
    {
        Task<Category> FindCategoryByName(string categoryName);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _dataContext;

        public CategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Category> FindCategoryByName(string categoryName)
        {
            return _dataContext.Categories.SingleOrDefault(x => x.Title == categoryName);
        }
    }
}
