using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;

namespace ShopMe.Repository.Repositories
{
    public interface IUserGroupRepository : IRepository<UserGroup>
    {

    }
    public class UserGroupRepository : RepositoryBase<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
