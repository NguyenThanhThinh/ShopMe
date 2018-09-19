using System.Collections.Generic;
using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Reository.Repositories;

namespace ShopMe.Business
{
    public interface IOrderDetailBusiness
    {
        IEnumerable<OrderDetail> GetListByOrderId(int id);
    }

    public class OrderDetailBusiness : IOrderDetailBusiness
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailBusiness(IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            _orderDetailRepository = orderDetailRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<OrderDetail> GetListByOrderId(int id)
        {
            return _orderDetailRepository.GetMulti(x => x.OrderID == id);
        }
    }
}