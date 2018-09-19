using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository;

namespace ShopMe.Reository.Repositories
{
  public  interface IProductUnitRepository:IRepository<ProductUnit>
    {
    }
    public class ProductUnitRepository : RepositoryBase<ProductUnit>, IProductUnitRepository
    {
        public ProductUnitRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
