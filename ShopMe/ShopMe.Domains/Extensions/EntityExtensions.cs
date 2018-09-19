using System;
using ShopMe.Entities.Models;

namespace ShopMe.Domains.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateBrand(this ProductBrand productBrand, ProductBrandViewModel productBrandVm)
        {
            productBrand.ProductBrandID = productBrandVm.ProductBrandID;
            productBrand.Name = productBrandVm.Name;
            productBrand.Phone = productBrandVm.Phone;
            productBrand.Address = productBrandVm.Address;
            productBrand.Email = productBrandVm.Email;
            productBrand.Description = productBrandVm.Description;
            productBrand.Alias = productBrandVm.Alias;
        }

        public static void UpdateFeedback(this Feedback feedback, FeedbackViewModel feedbackVm)
        {
            feedback.UserID = feedbackVm.UserID;
            feedback.ProductID = feedbackVm.ProductID;
            feedback.Content = feedbackVm.Content;
            feedback.CreatedDate = feedbackVm.CreatedDate;
            feedback.Title = feedbackVm.Title;
            feedback.Status = feedbackVm.Status;
        }

        public static void UpdateUnit(this ProductUnit productBrand, ProductUnitViewModel productBrandVm)
        {
            productBrand.ProductUnitID = productBrandVm.ProductUnitID;
            productBrand.Name = productBrandVm.Name;
            productBrand.Status = productBrandVm.Status;
        }

        public static void UpdateCategory(this ProductCategory category, ProductCategoryViewModel CategoryVm)
        {
            category.CategoryID = CategoryVm.CategoryID;
            category.DisplayOrder = CategoryVm.DisplayOrder;
            category.Name = CategoryVm.Name;
            category.ParentID = CategoryVm.ParentID;
            category.Status = CategoryVm.Status;
            category.Description = CategoryVm.Description;
            category.Alias = CategoryVm.Alias;
        }

        public static void UpdateSlider(this Slide slider, SliderViewModel SliderVM)
        {
            slider.ID = SliderVM.ID;
            slider.Image = SliderVM.Image;
            slider.Status = SliderVM.Status;
            slider.Description = SliderVM.Description;
            slider.DisplayOrder = SliderVM.DisplayOrder;
        }

        public static void UpdateApplicationGroup(this Group appGroup, GroupViewModel appGroupViewModel)
        {
            appGroup.ID = appGroupViewModel.ID;
            appGroup.Name = appGroupViewModel.Name;
            appGroup.Description = appGroupViewModel.Description;
        }

        public static void UpdateUser(this User appUser, UserViewModel appUserViewModel, string action = "add")
        {
            appUser.Id = appUserViewModel.Id;
            appUser.FullName = appUserViewModel.FullName;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
            appUser.Address = appUserViewModel.Address;
            appUser.IsDelete = appUserViewModel.IsDelete;
        }

        public static void UpdateApplicationRole(this Role appRole, RoleViewModel appRoleViewModel,
            string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
        }

        public static void UpdateProduct(this Product product, ProductViewModel ProductVm)
        {
            product.Name = ProductVm.Name;
            product.Alias = ProductVm.Alias;
            product.Quantity = ProductVm.Quantity;
            product.OriginalPrice = ProductVm.OriginalPrice;
            product.ProductBrandID = ProductVm.ProductBrandID;
            product.CategoryID = ProductVm.CategoryID;
            product.Status = ProductVm.Status;
            product.ProductUnitID = ProductVm.ProductUnitID;
            product.ProductCappacity = ProductVm.ProductCappacity;
            product.ViewCount = ProductVm.ViewCount;
            product.ViewRating = ProductVm.ViewRating;
            product.Content = ProductVm.Content;
            //product.Number = ProductVm.Number;
            product.Price = ProductVm.Price;
            //product.PromotionPrice = ProductVm.PromotionPrice;
            product.Description = ProductVm.Description;
            product.CreatedDate = ProductVm.CreatedDate;
            product.ProductRating = ProductVm.ProductRating;
            product.Image = ProductVm.Image;
        }

        public static void UpdateOrder(this Order order, OrderViewModel orderVm)
        {
            order.CustomerName = orderVm.CustomerName;
            order.CustomerAddress = orderVm.CustomerAddress;
            order.CustomerEmail = orderVm.CustomerEmail;
            order.CustomerMobile = orderVm.CustomerMobile;
            //order.CustomerMessage = orderVm.CustomerName;
            order.PaymentMethod = orderVm.PaymentMethod;
            order.CustomerDistricts = orderVm.CustomerDistricts;
            order.CustomerProvince = orderVm.CustomerProvince;
            order.CreatedDate = DateTime.Now;
            order.CreatedBy = orderVm.CreatedBy;
            order.Status = orderVm.Status;
            order.UserID = orderVm.UserID;
            order.PaymentStatus = orderVm.PaymentStatus;
        }


        public static void UpdateOrderCart(this Order order, ShoppingCartAdmin shopCartVm)
        {
            order.CreatedBy = shopCartVm.CreatedBy;
            order.CreatedDate = shopCartVm.CreatedDate;
            //order.CustomerBillingAddress = shopCartVm.CustomerBillingAddress;
            order.CustomerEmail = shopCartVm.CustomerEmail;
            order.UserID = shopCartVm.CustomerId;
            //order.CustomerMessage = shopCartVm.CustomerMessage;
            order.CustomerMobile = shopCartVm.CustomerMobile;
            order.CustomerName = shopCartVm.CustomerName;
            order.CustomerAddress = shopCartVm.CustomerAddress;
            order.PaymentMethod = shopCartVm.PaymentMethod;
        }

        public static void UpdateOrderDetail(this OrderDetail orderDetail, OrderDetailViewModel orderDetailVm)
        {
            orderDetail.OrderID = orderDetailVm.OrderID;
            orderDetail.Price = orderDetailVm.Price;
            orderDetail.ProductID = orderDetailVm.ProductID;
            //orderDetail.OriginalPrice = orderDetailVm.OriginalPrice;
            orderDetail.Quantity = orderDetailVm.Quantity;
        }
    }
}