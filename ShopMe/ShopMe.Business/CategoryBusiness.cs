using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Repository.Repositories;
using System.Collections.Generic;

namespace ShopMe.Business
{
    public interface ICategoryBusiness
    {
        ProductCategory Add(ProductCategory category);

        void Update(ProductCategory category);

        ProductCategory Delete(int id);

        IEnumerable<ProductCategory> GetAll();

        IEnumerable<ProductCategory> GetAll(string keyword);

        IEnumerable<ProductCategory> GetAllChildrend(int? id);

        IEnumerable<ProductCategory> GetAllByParentId(int? parentId);

        IEnumerable<ProductCategory> GetAllByParentId();

        ProductCategory GetById(int id);

        void Save();

    }
    public class CategoryBusiness : ICategoryBusiness
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryBusiness(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            this._categoryRepository = categoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public ProductCategory Add(ProductCategory category)
        {
            return _categoryRepository.Add(category);
        }

        public ProductCategory Delete(int id)
        {
            return _categoryRepository.Delete(id);
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public IEnumerable<ProductCategory> GetAll(string keyword)
        {
            return !string.IsNullOrEmpty(keyword) ? _categoryRepository.GetMulti(x => x.Name.Contains(keyword)) : _categoryRepository.GetAll();
        }

        public IEnumerable<ProductCategory> GetAllByParentId(int? parentId)
        {
            return _categoryRepository.GetMulti(x => x.Status && x.ParentID == parentId);
        }

        public IEnumerable<ProductCategory> GetAllByParentId()
        {
            return _categoryRepository.GetMulti(x => x.CategoryID == x.ParentID);
        }

        public IEnumerable<ProductCategory> GetAllChildrend(int? id)
        {
            return _categoryRepository.GetListByParameter(n => n.ParentID == id);


        }

        public ProductCategory GetById(int id)
        {
            return _categoryRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductCategory category)
        {
            _categoryRepository.Update(category);
        }
    }
}
