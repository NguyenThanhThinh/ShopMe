using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Reository.Repositories;
using ShopMe.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopMe.Business
{
    public interface IProductBusiness
    {
        Product Add(Product Product);

        void Update(Product Product);

        Product Delete(int id);
        Product Remove(int id);

        void IncreaseView(int id);


        IEnumerable<Product> GetAll();

        Product GetAllCBU(int product);
        IEnumerable<Product> GetAll(string keyword);

        IEnumerable<Product> GetProductLike(int like, int top);
        IEnumerable<Product> GetProductViewMax(int view, int top);

        IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize, string sort,
            out int totalRow);

        IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int? page, int pageSize, string sort);

        IEnumerable<Product> GetListProductByCategoryIdPaging(int id, int par, int page, int pageSize, string sort,
            out int totalRow);

        IEnumerable<Product> GetListProductALL(int categoryId, int page, int pageSize, string sort, out int totalRow);
        IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<Product> Search(string keyword, int? page, int pageSize, string sort);

        IEnumerable<ProductShowViewModel> getShowHot();
        IEnumerable<string> GetListProductByName(string name);

        Product GetById(int id);

        void Save();
        bool SellProduct(int productId, int quantity);
        bool KTQuantity(int productId, int quantity);
        IEnumerable<Product> SearchALL(string keyword, int page, int pageSize, string sort, out int totalRow);
        IEnumerable<Product> GetLastest(int top);
        IEnumerable<Product> GetHetHang(int top);
        IEnumerable<ProductShowViewModel> HotProduct(int top);
        IEnumerable<Product> GetHotProduct(int top);
        IEnumerable<Product> GetListProductByTag(string tagId, int page, int pagesize, out int totalRow);
        IEnumerable<DanhSachGoiSanPham> Getbylist(string user);
        IEnumerable<Product> GetListByCategory(int category);
        IEnumerable<Product> GetListBrandID(int productBrand);

        int CountProduct();
    }

    public class ProductBusiness : IProductBusiness
    {
        private IProductRepository _productRepository;

        private IListSugggetRepository _listsuggget;


        private IUnitOfWork _unitOfWork;

        public ProductBusiness(IProductRepository productRepository, IUnitOfWork unitOfWork,
            IListSugggetRepository listsuggget)
        {
            _productRepository = productRepository;
            _listsuggget = listsuggget;

            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Product> GetProductGoiY
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public Product Add(Product Product)
        {
            return _productRepository.Add(Product);
        }

        public Product Delete(int id)
        {
            return _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll(new string[] {"ProductCategory", "ProductBrand"});
        }

        public IEnumerable<Product> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _productRepository.GetMulti(x => x.Name.ToUpper().Contains(keyword.ToUpper()),
                    new string[] {"ProductCategory", "ProductBrand"});
            else
                return _productRepository.GetAll(new string[] {"ProductCategory", "ProductBrand"});
        }

        public IEnumerable<DanhSachGoiSanPham> Getbylist(string user)
        {
            return _listsuggget.GetListByParameter(n => n.UserID == user);
        }

        public Product GetById(int id)
        {
            return _productRepository.GetSingleById(id);
        }

        public IEnumerable<ProductShowViewModel> HotProduct(int top)
        {
            return _productRepository.getShowHot().OrderByDescending(x => x.ProductID).Take(top);
        }

        public IEnumerable<Product> GetLastest(int top)
        {
            return _productRepository.GetMulti(x => x.Status).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize,
            string sort, out int totalRow)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.CategoryID == categoryId);

            switch (sort)
            {
                case "09":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "90":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "ZA":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "AZ":
                    query = query.OrderBy(x => x.Name);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<string> GetListProductByName(string name)
        {
            return _productRepository.GetMulti(x => x.Status && x.Name.Contains(name)).Select(y => y.Name);
        }

        public IEnumerable<Product> GetListProductByTag(string tagId, int page, int pagesize, out int totalRow)
        {
            throw new NotImplementedException();
        }

        public void IncreaseView(int id)
        {
            var product = _productRepository.GetSingleById(id);
            if (product.ViewCount.HasValue)
                product.ViewCount += 1;
            else
                product.ViewCount = 1;
        }

        public Product Remove(int id)
        {
            var oldProduct = _productRepository.GetSingleById(id);

            _productRepository.Update(oldProduct);
            return oldProduct;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.Name.Contains(keyword));

            switch (sort)
            {
                case "09":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "90":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "ZA":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "AZ":
                    query = query.OrderBy(x => x.Name);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public bool SellProduct(int productId, int quantity)
        {
            var product = _productRepository.GetSingleById(productId);
            if (product.Quantity < quantity) return false;
            product.Quantity -= quantity;
            return true;
        }

        public void Update(Product Product)
        {
            _productRepository.Update(Product);
        }

        public IEnumerable<Product> SearchALL(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                var query = _productRepository.GetMulti(x => x.Status);

                switch (sort)
                {
                    case "09":
                        query = query.OrderBy(x => x.Price);
                        break;
                    case "90":
                        query = query.OrderBy(x => x.Price);
                        break;
                    case "ZA":
                        query = query.OrderByDescending(x => x.Name);
                        break;
                    case "AZ":
                        query = query.OrderBy(x => x.Name);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.CreatedDate);
                        break;
                }

                totalRow = query.Count();

                return query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var query1 = _productRepository.GetMulti(x => x.Status && x.Name.Contains(keyword));

            switch (sort)
            {
                case "09":
                    query1 = query1.OrderBy(x => x.Price);
                    break;
                case "90":
                    query1 = query1.OrderBy(x => x.Price);
                    break;
                case "ZA":
                    query1 = query1.OrderByDescending(x => x.Name);
                    break;
                case "AZ":
                    query1 = query1.OrderBy(x => x.Name);
                    break;
                default:
                    query1 = query1.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query1.Count();

            return query1.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetListProductALL(int categoryId, int page, int pageSize, string sort,
            out int totalRow)
        {
            if (categoryId == null)
            {
                var query1 = _productRepository.GetMulti(x => x.Status);
                switch (sort)
                {
                    case "09":
                        query1 = query1.OrderBy(x => x.Price);
                        break;
                    case "90":
                        query1 = query1.OrderBy(x => x.Price);
                        break;
                    case "ZA":
                        query1 = query1.OrderByDescending(x => x.Name);
                        break;
                    case "AZ":
                        query1 = query1.OrderBy(x => x.Name);
                        break;
                    default:
                        query1 = query1.OrderByDescending(x => x.CreatedDate);
                        break;
                }

                totalRow = query1.Count();

                return query1.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var query = _productRepository.GetMulti(x => x.Status && x.CategoryID == categoryId);
            switch (sort)
            {
                case "09":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "90":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "ZA":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "AZ":
                    query = query.OrderBy(x => x.Name);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<ProductShowViewModel> getShowHot()
        {
            return _productRepository.getShowHot();
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int id, int par, int page, int pageSize,
            string sort, out int totalRow)
        {
            var query = _productRepository.GetMulti(n => n.CategoryID == id && n.ProductCategory.ParentID == id,
                new string[] {"ProductCategory"});

            switch (sort)
            {
                case "09":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "90":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "ZA":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "AZ":
                    query = query.OrderBy(x => x.Name);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int? page, int pageSize,
            string sort)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.CategoryID == categoryId);

            switch (sort)
            {
                case "09":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "90":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "ZA":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "AZ":
                    query = query.OrderBy(x => x.Name);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            return query;
        }

        public IEnumerable<Product> Search(string keyword, int? page, int pageSize, string sort)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.Name.Contains(keyword));

            switch (sort)
            {
                case "09":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "90":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "ZA":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "AZ":
                    query = query.OrderBy(x => x.Name);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            return query;
        }

        public Product GetAllCBU(int product)
        {
            return _productRepository.GetSingleByCondition(n => n.ProductID == product,
                new string[] {"ProductCategory", "ProductBrand", "ProductUnit"});
        }

        public IEnumerable<Product> GetListByCategory(int category)
        {
            return _productRepository.GetListByParameter(n => n.Status && n.CategoryID == category)
                .OrderByDescending(n => n.CreatedDate);
        }

        public IEnumerable<Product> GetListBrandID(int productBrand)
        {
            return _productRepository
                .GetMulti(n => n.Status && n.ProductBrandID == productBrand, new string[] {"ProductBrand"})
                .OrderByDescending(n => n.CreatedDate);
        }

        public IEnumerable<Product> GetProductLike(int like, int top)
        {
            return _productRepository.GetListByParameter(n => n.ViewRating >= like)
                .OrderByDescending(n => n.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetProductViewMax(int view, int top)
        {
            return _productRepository.GetMulti(n => n.ViewCount > view).OrderByDescending(n => n.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetHotProduct(int top)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetHetHang(int top)
        {
            var model = _productRepository.GetMulti(n => n.Quantity <= 0).OrderByDescending(n => n.CreatedDate)
                .Take(top);
            if (model != null)
                return model;
            return null;
        }

        public bool KTQuantity(int productId, int quantity)
        {
            var product = _productRepository.GetSingleById(productId);
            if (product.Quantity < quantity) return false;

            return true;
        }

        public int CountProduct()
        {
            return _productRepository.Count(n => n.Status);
        }
    }
}