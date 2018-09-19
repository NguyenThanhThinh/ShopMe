using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Reository.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ShopMe.Business
{
    public interface IProductBrandBusiness
    {
        ProductBrand Add(ProductBrand category);

        void Update(ProductBrand category);

        ProductBrand Delete(int id);

        IEnumerable<ProductBrand> GetAll();

        IEnumerable<ProductBrand> GetAll(string keyword);

        IEnumerable<ProductBrand> GetAllByParentId(int parentId);

        ProductBrand GetById(int id);

        int CountBrand();

        void Save();
    }
    public class ProductBrandBusiness : IProductBrandBusiness
    {
        private readonly IProductBrandRepository _productBrandRepository;
        private readonly IUnitOfWork _unofWork;

        public ProductBrandBusiness(IProductBrandRepository productBrandRepository, IUnitOfWork UnofWork)
        {
            this._productBrandRepository = productBrandRepository;
            this._unofWork = UnofWork;
        }

        public ProductBrand Add(ProductBrand category)
        {
            return _productBrandRepository.Add(category);
        }

        public int CountBrand()
        {
            return _productBrandRepository.GetAll().Count();
        }

        public ProductBrand Delete(int id)
        {
            return _productBrandRepository.Delete(id);
        }

        public IEnumerable<ProductBrand> GetAll()
        {
            return _productBrandRepository.GetAll();
        }

        public IEnumerable<ProductBrand> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _productBrandRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _productBrandRepository.GetAll();
        }

        public IEnumerable<ProductBrand> GetAllByParentId(int parentId)
        {
            return _productBrandRepository.GetMulti(x=>x.ProductBrandID==parentId);
        }

        public ProductBrand GetById(int id)
        {
            return _productBrandRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unofWork.Commit();
        }

        public void Update(ProductBrand category)
        {
            _productBrandRepository.Update(category);
        }
    }
}
