using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository.Repositories;
using System.Collections.Generic;

namespace ShopMe.Business
{
    public interface ISystemConfigBusiness
    {
        SystemConfig GetSystemConfig(string code);
        IEnumerable<SystemConfig> GetAllSystemConfig();
        void Create(string code, string strValue, int? intValue);
        void SaveChange();

    }
    public class SystemConfigBusiness
    {
        readonly ISystemConfigRepository _systemConfigRepository;
        readonly IUnitOfWork _unitOfWork;
        public SystemConfigBusiness(ISystemConfigRepository systemConfigRepository, IUnitOfWork unitOfWork)
        {
            _systemConfigRepository = systemConfigRepository;
            _unitOfWork = unitOfWork;
        }
        public SystemConfig GetSystemConfig(string code)
        {
            return _systemConfigRepository.GetSingleByCondition(x => x.Code == code);
        }

        public IEnumerable<SystemConfig> GetAllSystemConfig()
        {
            return _systemConfigRepository.GetAll();
        }

        public void Create(string code, string strValue, int? intValue)
        {

            SystemConfig sysConfig = new SystemConfig()
            {
                Code = code,
                ValueString = strValue,
                ValueInt = intValue
            };
            _systemConfigRepository.Add(sysConfig);

        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }
    }
}
