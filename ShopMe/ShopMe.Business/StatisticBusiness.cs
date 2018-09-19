using ShopMe.Domains;
using ShopMe.Reository.Interface;
using ShopMe.Reository.Repositories;
using System.Collections.Generic;

namespace ShopMe.Business
{
    public interface IStatisticBusiness
    {
        IEnumerable<StatisticPriceQuantity> GetStatisticPriceQuantity(string fromDate, string toDate);
        IEnumerable<ProductStatisticViewModel> GetProductStatistic(string fromDate, string toDate);
    }

    public class StatisticBusiness : IStatisticBusiness
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly IUnitOfWork _UnofWork;
        public StatisticBusiness( IOrderRepository OrderRepository, IUnitOfWork UnofWork)
        {
            this._OrderRepository = OrderRepository;
            this._UnofWork = UnofWork;
        }

        public IEnumerable<StatisticPriceQuantity> GetStatisticPriceQuantity(string fromDate, string toDate)
        {
            return _OrderRepository.GetStatisticPriceQuantity(fromDate,toDate);
        }
        public IEnumerable<ProductStatisticViewModel> GetProductStatistic(string fromDate, string toDate)
        {
            return _OrderRepository.GetProductStatistic(fromDate, toDate);
        }

    }
}
