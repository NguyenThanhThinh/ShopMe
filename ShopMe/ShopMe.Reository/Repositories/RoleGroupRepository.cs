using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;

namespace ShopMe.Repository.Repositories
{
    public interface IRoleGroupRepository : IRepository<RoleGroup>
    {

    }
    public class RoleGroupRepository : RepositoryBase<RoleGroup>, IRoleGroupRepository
    {
        public RoleGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
