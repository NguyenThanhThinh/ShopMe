using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository;

namespace ShopMe.Reository.Repositories
{
   public interface IProductDetailRepository:IRepository<ProductDetail>
    {
    }
    public class ProductDetailRepository : RepositoryBase<ProductDetail>, IProductDetailRepository
    {
        public ProductDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
