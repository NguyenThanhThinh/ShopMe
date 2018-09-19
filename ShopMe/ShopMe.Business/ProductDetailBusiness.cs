using ShopMe.Entities.Models;
using ShopMe.Reository.Interface;
using ShopMe.Reository.Repositories;
using ShopMe.Repository.Repositories;
using System;
using System.Collections.Generic;

namespace ShopMe.Business
{
    public  interface IProductDetailBusiness
    {
        Product Create(ref Product product, List<ProductDetail> productDetails);
        void Create(List<ProductDetail> attributeValue, int productID);
        void Save();
    }
    public class ProductDetailBusiness : IProductDetailBusiness
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductDetailRepository _productDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductDetailBusiness(IProductDetailRepository productDetailRepository, IProductRepository productRepository, IUnitOfWork unOfWork)
        {
            this._productRepository = productRepository;
            this._productDetailRepository = productDetailRepository;

            this._unitOfWork = unOfWork;
        }

        public Product Create(ref Product product, List<ProductDetail> productDetails)
        {
            try
            {
                _productRepository.Add(product);
                _unitOfWork.Commit();

                foreach (var productDetail in productDetails)
                {
                    productDetail.ProductID = product.ProductID;
                    _productDetailRepository.Add(productDetail);

                }
                _unitOfWork.Commit();
                return product;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Create(List<ProductDetail> attributeValue, int productID)
        {
            foreach (var productAttributeValue in attributeValue)
            {
                _productDetailRepository.Add(productAttributeValue);
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
