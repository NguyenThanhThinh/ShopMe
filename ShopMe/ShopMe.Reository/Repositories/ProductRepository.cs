using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using System.Collections.Generic;

namespace ShopMe.Repository.Repositories
{
    public interface  IProductRepository:IRepository<Product>
    {
        IEnumerable<ProductShowViewModel> getShowHot();
    }
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ProductShowViewModel> getShowHot()
        {
            return DbContext.Database.SqlQuery<ProductShowViewModel>("ProductHotSP");
        }
    }
}
