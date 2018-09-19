using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Reository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Business
{

   public interface IProductUnitBusiness
    {
        ProductUnit Add(ProductUnit category);

        void Update(ProductUnit category);

        ProductUnit Delete(int id);

        IEnumerable<ProductUnit> GetAll();

        IEnumerable<ProductUnit> GetAll(string keyword);

        IEnumerable<ProductUnit> GetAllByParentId(int parentId);

        ProductUnit GetById(int id);

        void Save();
    }
   public class ProductUnitBusiness:IProductUnitBusiness
    {
        private readonly IProductUnitRepository _productUnitRepository;
        private readonly IUnitOfWork _UnitofWork;
        public ProductUnitBusiness(IProductUnitRepository productUnitRepository, IUnitOfWork UnitofWork)
        {
            _productUnitRepository = productUnitRepository;
            _UnitofWork = UnitofWork;
        }

        public ProductUnit Add(ProductUnit category)
        {
            return _productUnitRepository.Add(category);
        }

        public ProductUnit Delete(int id)
        {
            return _productUnitRepository.Delete(id);
        }

        public IEnumerable<ProductUnit> GetAll()
        {
            return _productUnitRepository.GetAll();
        }

        public IEnumerable<ProductUnit> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _productUnitRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _productUnitRepository.GetAll();
        }

        public IEnumerable<ProductUnit> GetAllByParentId(int parentId)
        {
            return _productUnitRepository.GetMulti(x=>x.Status && x.ProductUnitID == parentId);
        }

        public ProductUnit GetById(int id)
        {
            return _productUnitRepository.GetSingleById(id);
        }

        public void Save()
        {
            _UnitofWork.Commit();
        }

        public void Update(ProductUnit category)
        {
            _productUnitRepository.Update(category);
        }
    }
}
