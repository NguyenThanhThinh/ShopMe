using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository;

namespace ShopMe.Reository.Repositories
{
    public interface IDistrictsRepository : IRepository<Districts>
    {
      

    }
    public class DistrictsRepository : RepositoryBase<Districts>, IDistrictsRepository
    {
        public DistrictsRepository(IDbFactory dbFactory) : base(dbFactory)
        {


        }
      


    }
}
