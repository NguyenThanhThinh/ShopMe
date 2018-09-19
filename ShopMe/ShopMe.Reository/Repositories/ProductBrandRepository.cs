using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Reository.Repositories
{
    public interface IProductBrandRepository: IRepository<ProductBrand>
    {
    }

    public class ProductBrandRepository : RepositoryBase<ProductBrand>, IProductBrandRepository
    {
        public ProductBrandRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
