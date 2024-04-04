using Core.Entities;
using Infrastructure.DataContext;

namespace Infrastructure.Repositories.Categories
{
    public class CategoryRepository(TuitterContext context) : ICategoryRepository
    {
        public IEnumerable<Category> FindCategoryByName(IEnumerable<string> categoriesNames)
        {
            var result = new List<Category>();
            foreach (var category in categoriesNames)
            {
                var cat = context.Categories.FirstOrDefault(x => x.Title.ToLower() == category.ToLower());

                if (cat is null)
                {
                    cat = new Category { Title = category };
                    context.Categories.Add(cat);
                    context.SaveChanges();
                }
                yield return cat;
            }
        }
    }
}
