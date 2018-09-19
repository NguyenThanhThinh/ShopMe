using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository;

namespace ShopMe.Reository.Repositories
{
   public interface ISliderRepository: IRepository<Slide>
    {
    }
    public class SliderRepository : RepositoryBase<Slide>, ISliderRepository
    {
        public SliderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }

}
