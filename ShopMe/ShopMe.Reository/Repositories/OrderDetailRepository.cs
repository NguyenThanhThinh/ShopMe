using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ShopMe.Reository.Repositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        IEnumerable<OrderDetail> GetDetailByOrderId(int id);
    }

    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<OrderDetail> GetDetailByOrderId(int id)
        {
            var query = from o in DbContext.OrderDetails
                where o.OrderID == id
                select o;
            return query.Include(a => a.Product).OrderBy(x => x.ProductID);
        }
    }
}