using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Reository.Repositories;
using ShopMe.Repository.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ShopMe.Business
{
    public interface IFeedbackBusiness
    {
        Feedback Add(Feedback category);

        void Update(Feedback category);

        Feedback Delete(int id);
        IEnumerable<Feedback> GetAll();
        IEnumerable<Feedback> GetAll(int product);

        void UserRating(string user);
        Feedback GetById(int id);
        IEnumerable<Feedback> GetAll(string keyword);

        IEnumerable<Feedback> GetAllByParentId(string parentId);

        bool GetId(int product);

        int Count(int ID);

        bool GetId(string user);
        int GetNumberFeedback();
        void Save();
    }

    public class FeedbackBusiness : IFeedbackBusiness
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private IProductRepository _productRepository;
        private readonly IUnitOfWork _uniofWork;

        public FeedbackBusiness(IFeedbackRepository feedbackRepository, IUnitOfWork uniofWork,
            IProductRepository productRepository)
        {
            _feedbackRepository = feedbackRepository;
            _productRepository = productRepository;
            _uniofWork = uniofWork;
        }

        public Feedback Add(Feedback category)
        {
            return _feedbackRepository.Add(category);
        }


        public int Count(int id)
        {
            var model = _feedbackRepository.GetAll().Count();
            return model;
        }

        public Feedback Delete(int id)
        {
            return _feedbackRepository.Delete(id);
        }

        public IEnumerable<Feedback> GetAll(int product)
        {
            return _feedbackRepository.GetMulti(n => n.ProductID == product, new string[] {"User"})
                .OrderBy(n => n.CreatedDate);
        }

        public IEnumerable<Feedback> GetAll(string keyword)
        {
            return !string.IsNullOrEmpty(keyword)
                ? _feedbackRepository.GetMulti(x => x.Title.Contains(keyword))
                : _feedbackRepository.GetAll(new string[] {"User", "Product"}).OrderBy(n => n.CreatedDate);
        }

        public IEnumerable<Feedback> GetAll()
        {
            return _feedbackRepository.GetAll();
        }

        public IEnumerable<Feedback> GetAllByParentId(string parentId)
        {
            return _feedbackRepository.GetListByParameter(x => x.UserID == parentId);
        }

        public Feedback GetById(int id)
        {
            return _feedbackRepository.GetSingleByCondition(n => n.FeedbackID == id, new string[] {"User"});
        }

        public bool GetId(int product)
        {
            var model = _feedbackRepository.GetSingleById(product);
            if (model != null) return true;

            return false;
        }

        public bool GetId(string user)
        {
            var model = _feedbackRepository.GetSingleById(user);
            if (model != null) return true;

            return false;
        }

        public int GetNumberFeedback()
        {
            var count = _feedbackRepository.Count(n => n.Status != false);
            if (count > 0) return count;

            return 0;
        }

        public void Save()
        {
            _uniofWork.Commit();
        }

        public void Update(Feedback category)
        {
            _feedbackRepository.Update(category);
        }

        public void UserRating(string user)
        {
            _feedbackRepository.RatingUser(user);
            _uniofWork.Commit();
        }
    }
}