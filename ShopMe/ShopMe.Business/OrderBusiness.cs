using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Reository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopMe.Business
{
    public interface IOrderBusiness
    {
        Order Create(ref Order order, List<OrderDetail> orderDetails);
        void Update(Order order);

        Order Delete(int id);
        void UpdateStatus(int orderId);
        IEnumerable<Order> GetAll();

        IEnumerable<Order> GetAll(string keyword);

        Order GetById(int id);

        Order GetByEmailAndId(string email, int id);

        IEnumerable<Order> GetListByEmail(string email);

        IEnumerable<Order> GetListByCustomerId(string id);

        IEnumerable<OrderDetail> GetListDetailById(int id);
        OrderDetail GetByID(int id);
        OrderDetail GetByDetail(int id);
        IEnumerable<Order> GetLatestOrder(int top);

        int GetNumberOrderNew();

        void Save();
    }

    public class OrderBusiness : IOrderBusiness
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ICommonBusiness _commonbusiness;
        private readonly IUnitOfWork _unitOfWork;

        public OrderBusiness(IOrderRepository orderRepository, ICommonBusiness commonbusiness,
            IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _commonbusiness = commonbusiness;
            _unitOfWork = unitOfWork;
        }


        public Order Create(ref Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.OrderID;
                    _orderDetailRepository.Add(orderDetail);
                }

                return order;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void Update(Order order)
        {
            _orderRepository.Update(order);
        }

        public Order Delete(int id)
        {
            return _orderRepository.Delete(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public Order GetById(int id)
        {
            return _orderRepository.GetSingleById(id);
        }

        public IEnumerable<Order> GetListByEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
                return _orderRepository.GetMulti(x =>
                    x.CustomerEmail.ToUpper() == email.ToUpper() || x.User.Email.ToUpper() == email.ToUpper());
            else
                return _orderRepository.GetAll();
        }

        public IEnumerable<Order> GetListByCustomerId(string id)
        {
            return _orderRepository.GetMulti(x => x.User.Id.ToUpper() == id.ToUpper());
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public Order GetByEmailAndId(string email, int id)
        {
            if (!string.IsNullOrEmpty(email))
                return _orderRepository.GetSingleByCondition(x =>
                    x.CustomerEmail.ToUpper() == email.ToUpper() && x.OrderID == id);
            else
                return GetById(id);
        }

        public IEnumerable<Order> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _orderRepository.GetMulti(x =>
                    x.CustomerName.ToUpper().Contains(keyword.ToUpper()) ||
                    x.CustomerEmail.ToUpper().Contains(keyword.ToUpper()));
            else
                return _orderRepository.GetAll();
        }

        public IEnumerable<OrderDetail> GetListDetailById(int id)
        {
            return _orderDetailRepository.GetDetailByOrderId(id);
        }

        public int GetNumberOrderNew()
        {
            var value = _commonbusiness.GetSystemConfig("Pending").ValueInt;
            if (value == null) return 0;

            return _orderRepository.GetMulti(x => x.Status == value).Count();
        }

        public IEnumerable<Order> GetLatestOrder(int top)
        {
            return _orderRepository.GetAll().OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public OrderDetail GetByID(int id)
        {
            return _orderDetailRepository.GetSingleById(id);
        }

        public void UpdateStatus(int orderId)
        {
            var order = _orderRepository.GetSingleById(orderId);
            order.Status = 21;
            _orderRepository.Update(order);
        }

        public OrderDetail GetByDetail(int id)
        {
            var model = _orderDetailRepository.GetSingleByCondition(n => n.OrderID == id);
            return model;
        }
    }
}