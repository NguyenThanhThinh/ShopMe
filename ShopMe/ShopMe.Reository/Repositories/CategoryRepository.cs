using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;

namespace ShopMe.Repository.Repositories
{
    public interface ICategoryRepository : IRepository<ProductCategory>
    {

    }

    public class CategoryRepository : RepositoryBase<ProductCategory>, ICategoryRepository
    {
        public CategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
