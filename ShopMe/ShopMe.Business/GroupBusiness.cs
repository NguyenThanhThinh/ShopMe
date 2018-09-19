using ShopMe.Domains.Common;
using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ShopMe.Business
{
    public interface IGroupBusiness
    {
        Group GetDetail(int id);

        IEnumerable<Group> GetAll(int page, int pageSize, out int totalRow, string filter);

        IEnumerable<Group> GetAll();

        Group Add(Group appGroup);

        void Update(Group appGroup);

        Group Delete(int id);

        bool AddUserToGroups(IEnumerable<UserGroup> groups, string userId);

        bool AddUserToGroups(UserGroup groups, string userId);

        IEnumerable<Group> GetListGroupByUserId(string userId);

        IEnumerable<User> GetListUserByGroupId(int groupId);

        Group GetByGroupName(string name);

        void Save();
    }

    public class GroupBusiness : IGroupBusiness
    {
        private readonly IGroupRepository _appGroupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserGroupRepository _appUserGroupRepository;

        public GroupBusiness(IUnitOfWork unitOfWork,
            IUserGroupRepository appUserGroupRepository,
            IGroupRepository appGroupRepository)
        {
            _appGroupRepository = appGroupRepository;
            _appUserGroupRepository = appUserGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public Group Add(Group appGroup)
        {
            if (_appGroupRepository.CheckContains(x => x.Name == appGroup.Name))
                throw new NameDuplicatedException(ConfigHelper.GetByKey("tên không được trùng"));
            return _appGroupRepository.Add(appGroup);
        }

        public Group Delete(int id)
        {
            var appGroup = _appGroupRepository.GetSingleById(id);
            return _appGroupRepository.Delete(appGroup);
        }

        public IEnumerable<Group> GetAll()
        {
            return _appGroupRepository.GetAll();
        }

        public IEnumerable<Group> GetAll(int page, int pageSize, out int totalRow, string filter = null)
        {
            var query = _appGroupRepository.GetAll();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.ToUpper().Contains(filter.ToUpper()));

            totalRow = query.Count();
            return query.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);
        }

        public Group GetDetail(int id)
        {
            return _appGroupRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Group appGroup)
        {
            if (_appGroupRepository.CheckContains(x => x.Name == appGroup.Name && x.ID != appGroup.ID))
                throw new NameDuplicatedException(ConfigHelper.GetByKey("tên không được trùng"));
            _appGroupRepository.Update(appGroup);
        }

        public bool AddUserToGroups(IEnumerable<UserGroup> userGroups, string userId)
        {
            _appUserGroupRepository.DeleteMulti(x => x.UserId == userId);
            foreach (var userGroup in userGroups) _appUserGroupRepository.Add(userGroup);

            return true;
        }

        public IEnumerable<Group> GetListGroupByUserId(string userId)
        {
            return _appGroupRepository.GetListGroupByUserId(userId);
        }

        public IEnumerable<User> GetListUserByGroupId(int groupId)
        {
            return _appGroupRepository.GetListUserByGroupId(groupId);
        }

        public Group GetByGroupName(string name)
        {
            return _appGroupRepository.GetMulti(x => x.Name == name).FirstOrDefault();
        }

        public bool AddUserToGroups(UserGroup groups, string userId)
        {
            _appUserGroupRepository.DeleteMulti(x => x.UserId == userId);
            _appUserGroupRepository.Add(groups);
            return true;
        }
    }
}