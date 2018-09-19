using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Reository.Repositories;
using ShopMe.Repository.Repositories;
using System.Collections.Generic;

namespace ShopMe.Business
{
    public  interface ICommonBusiness
    {

        SystemConfig GetSystemConfig(string code);
        IEnumerable<SystemConfig> GetAllSystemConfig();
        void Create(string code, string strValue, int? intValue);
        void SaveChange();
        IEnumerable<Districts> GetAll();
        
        IEnumerable<Provinces> GetAllProvinces();
        IEnumerable<Districts> GetByDistricts(int id);
        Provinces GetByProvinces(int id);
    }
    public class CommonBusiness : ICommonBusiness
    {
        private readonly IProvincesRepository _provicesRepository;
        private readonly IDistrictsRepository _districtsRepository;
        readonly ISystemConfigRepository _systemConfigRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CommonBusiness(IProvincesRepository provicesRepository, IDistrictsRepository districtsRepository, ISystemConfigRepository systemConfigRepository, IUnitOfWork unitOfWork)
        {
            this._provicesRepository = provicesRepository;
            this._districtsRepository = districtsRepository;
            this._systemConfigRepository = systemConfigRepository;
            this._unitOfWork = unitOfWork;

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
        public IEnumerable<Districts> GetAll()
        {
           return _districtsRepository.GetAll();
        }

     
        public IEnumerable<Provinces> GetAllProvinces()
        {
           return _provicesRepository.GetAll();
        }

        public IEnumerable<Districts> GetByDistricts(int id)
        {
            return _districtsRepository.GetListByParameter(n => n.ProvinceID == id);
        }

        public Provinces GetByProvinces(int id)
        {
            return _provicesRepository.GetSingleById(id);
        }

      
    }
}
