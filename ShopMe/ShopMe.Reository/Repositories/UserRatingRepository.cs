using ShopMe.Domains;
using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ShopMe.Reository.Repositories
{
    public interface IUserRatingRepository : IRepository<UserRating>
    {
        IEnumerable<RatingProductViewModel> UserRating(int product, string user, int rating);
        IEnumerable<UserRating> GetmutiRating(int product, string user);
        UserRating getExits(int product, string user);

    }

    public class UserRatingRepository : RepositoryBase<UserRating>, IUserRatingRepository
    {
        public UserRatingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public UserRating getExits(int product, string user)
        {
            var model = DbContext.UserRatings.Single(n => n.ProductID == product && n.UserID == user);
            return model;
        }

        public IEnumerable<UserRating> GetmutiRating(int product, string user)
        {
           var model= DbContext.UserRatings.Where(n=>n.ProductID==product&&n.UserID==user).ToList();
           return model;
          
        }

        public IEnumerable<RatingProductViewModel> UserRating(int product, string user, int rating)
        {

            var parameters = new SqlParameter[]{
                   new SqlParameter("@ProductID", product) ,
                    new SqlParameter("@UserID", user),
                    new SqlParameter("@Rating",rating)

            };
            return DbContext.Database.SqlQuery<RatingProductViewModel>("RatingProductSP @ProductID, @UserID, @Rating", parameters);
        }
           
        
    }
}
