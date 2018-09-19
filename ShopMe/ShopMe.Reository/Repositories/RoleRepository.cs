using System.Collections.Generic;
using System.Linq;
using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;

namespace ShopMe.Repository.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        IEnumerable<Role> GetListRoleByGroupId(int groupId);                                                                   
    }
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<Role> GetListRoleByGroupId(int groupId)
        {
            var query = from g in DbContext.Roles
                        join ug in DbContext.RoleGroups
                        on g.Id equals ug.RoleId
                        where ug.GroupId == groupId
                        select g;
            return query;
        }                                      
    }
}
