using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository;

namespace ShopMe.Reository.Repositories
{
    public  interface IProvincesRepository: IRepository<Provinces>
    {
       
    }
    public class ProvincesRepository : RepositoryBase<Provinces>, IProvincesRepository
    {
        public ProvincesRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
       
}
