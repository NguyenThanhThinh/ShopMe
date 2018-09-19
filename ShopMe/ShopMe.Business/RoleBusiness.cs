using ShopMe.Domains.Common;
using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ShopMe.Business
{
    public interface IRoleBusiness
    {
        Role GetDetail(string id);

        IEnumerable<Role> GetAll(int page, int pageSize, out int totalRow, string filter);

        IEnumerable<Role> GetAll();

        Role Add(Role appRole);

        void Update(Role AppRole);

        void Delete(string id);

        //Add roles to a sepcify group
        bool AddRolesToGroup(IEnumerable<RoleGroup> roleGroups, int groupId);

        //Get list role by group id
        IEnumerable<Role> GetListRoleByGroupId(int groupId);
                                                                                                                   
        void Save();
    }

    public class RoleBusiness : IRoleBusiness
    {
        private IRoleRepository _appRoleRepository;
        private IRoleGroupRepository _appRoleGroupRepository;
        private IUnitOfWork _unitOfWork;                      

        public RoleBusiness(IUnitOfWork unitOfWork,
            IRoleRepository appRoleRepository, IRoleGroupRepository appRoleGroupRepository)
        {
            _appRoleRepository = appRoleRepository;
            _appRoleGroupRepository = appRoleGroupRepository;  
            _unitOfWork = unitOfWork;
        }

        public Role Add(Role appRole)
        {
            if (_appRoleRepository.CheckContains(x => x.Description == appRole.Description))
                throw new NameDuplicatedException(Domains.Common.ConfigHelper.GetByKey("DuplicatedName"));
            return _appRoleRepository.Add(appRole);
        }

        public bool AddRolesToGroup(IEnumerable<RoleGroup> roleGroups, int groupId)
        {
            _appRoleGroupRepository.DeleteMulti(x => x.GroupId == groupId);
            foreach (var roleGroup in roleGroups)
            {
                _appRoleGroupRepository.Add(roleGroup);
            }
            return true;
        }

        public void Delete(string id)
        {
            _appRoleRepository.DeleteMulti(x => x.Id == id);
        }

        public IEnumerable<Role> GetAll()
        {
            return _appRoleRepository.GetAll().OrderBy(x=>x.Description);
        }

        public IEnumerable<Role> GetAll(int page, int pageSize, out int totalRow, string filter = null)
        {
            var query = _appRoleRepository.GetAll();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Description.ToUpper().Contains(filter.ToUpper()) || x.Name.ToUpper().Contains(filter.ToUpper()));

            totalRow = query.Count();
            return query.OrderBy(x => x.Description).Skip(page * pageSize).Take(pageSize);
        }

        public Role GetDetail(string id)
        {
            return _appRoleRepository.GetSingleByCondition(x => x.Id == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Role AppRole)
        {
            if (_appRoleRepository.CheckContains(x => x.Description == AppRole.Description && x.Id != AppRole.Id))
                throw new NameDuplicatedException(ConfigHelper.GetByKey("DuplicatedName"));
            _appRoleRepository.Update(AppRole);
        }

        public IEnumerable<Role> GetListRoleByGroupId(int groupId)
        {
            return _appRoleRepository.GetListRoleByGroupId(groupId);
        }

       
    }
}
