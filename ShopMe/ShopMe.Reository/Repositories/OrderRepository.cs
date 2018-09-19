using ShopMe.Domains;
using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ShopMe.Reository.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<StatisticPriceQuantity> GetStatisticPriceQuantity(string fromDate, string toDate);
        IEnumerable<ProductStatisticViewModel> GetProductStatistic(string fromDate, string toDate);
    }

    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ProductStatisticViewModel> GetProductStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<ProductStatisticViewModel>("GetProductStatisticsSP @fromDate, @toDate",
                parameters);
        }

        public IEnumerable<StatisticPriceQuantity> GetStatisticPriceQuantity(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<StatisticPriceQuantity>("GetRevenueStatisticSP @fromDate, @toDate",
                parameters);
        }
    }
}