using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ShopMe.Repository.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        IEnumerable<Group> GetListGroupByUserId(string userId);
        IEnumerable<User> GetListUserByGroupId(int groupId);
    }
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Group> GetListGroupByUserId(string userId)
        {
            var query = from g in DbContext.Groups
                        join ug in DbContext.UserGroups
                        on g.ID equals ug.GroupId
                        where ug.UserId == userId
                        select g;
            return query;
        }

        public IEnumerable<User> GetListUserByGroupId(int groupId)
        {
            var query = from g in DbContext.Groups
                        join ug in DbContext.UserGroups
                        on g.ID equals ug.GroupId
                        join u in DbContext.Users
                        on ug.UserId equals u.Id
                        where ug.GroupId == groupId
                        select u;
            return query;
        }
    }
}
