using ShopMe.Domains;
using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Reository.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ShopMe.Business
{
    public interface IUserRatingBusiness
    {
        UserRating Add(UserRating userRating);
        void RatingProductInsert(UserRating userRating);

        List<UserRating> GetListRatingByProduct(int productId);
        IEnumerable<RatingProductViewModel> UserRating(int product, string user, int rating);

        IEnumerable<DanhSachGoiSanPham> GetAll();
        IEnumerable<UserRating> GetmutiRating(int product, string user);
        UserRating getExist(int product, string user);
        int Count(int rating);


        void Save();
    }

    public class UserRatingBusiness : IUserRatingBusiness
    {
        private readonly IUserRatingRepository _userRatingRepository;
        private readonly IListSugggetRepository _listsuggget;


        private IUnitOfWork _unitOfWork;

        public UserRatingBusiness(IUserRatingRepository userRatingRepository, IUnitOfWork unitOfWork,
            IListSugggetRepository listsuggget)
        {
            this._userRatingRepository = userRatingRepository;
            this._listsuggget = listsuggget;
            this._unitOfWork = unitOfWork;
        }

        public UserRating Add(UserRating userRating)
        {
            return _userRatingRepository.Add(userRating);
        }

        public int Count(int rating)
        {
            return _userRatingRepository.Count(n => n.Rating == rating);
        }


        public IEnumerable<DanhSachGoiSanPham> GetAll()
        {
            return _listsuggget.GetAll();
        }

        public UserRating getExist(int product, string user)
        {
            return _userRatingRepository.GetSingleByCondition(n => n.ProductID == product && n.UserID == user);
        }

        public IEnumerable<UserRating> GetListAll()
        {
            return _userRatingRepository.GetAll();
        }

        public List<UserRating> GetListRatingByProduct(int productId)
        {
            return _userRatingRepository.GetListByParameter(n => n.ProductID == productId).ToList();
        }

        public IEnumerable<UserRating> GetmutiRating(int product, string user)
        {
            return _userRatingRepository.GetmutiRating(product, user);
        }

        public void RatingProductInsert(UserRating userRating)
        {
            var model = _userRatingRepository.GetListByParameter(n =>
                n.UserID == userRating.UserID && n.ProductID == userRating.ProductID);
            if (model.Count() > 0)
            {
                _userRatingRepository.Update(userRating);
            }
        }

        //public UserRating RatingProductInsert(string user, int product)
        //{
        //    if( user != null && product != 0){
        //        _userRatingRepository.Add()
        //    }
        //}

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<RatingProductViewModel> UserRating(int product, string user, int rating)
        {
            return _userRatingRepository.UserRating(product, user, rating);
        }
    }
}