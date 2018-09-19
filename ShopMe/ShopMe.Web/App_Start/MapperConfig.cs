using AutoMapper;
using ShopMe.Domains;
using ShopMe.Entities.Models;

namespace ShopMe.Web.App_Start
{
    public class MapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Product, ProductViewModel>();
                cfg.CreateMap<ProductCategory, ProductCategoryViewModel>();
                cfg.CreateMap<Slide, SliderViewModel>();
                cfg.CreateMap<Districts, DistrictsViewModel>();
                cfg.CreateMap<Provinces, ProvincesViewModel>();
                cfg.CreateMap<Order, OrderViewModel>();
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<Role, RoleViewModel>();
                cfg.CreateMap<Group, GroupViewModel>();
                cfg.CreateMap<OrderDetail, OrderDetailViewModel>();
                cfg.CreateMap<Order, OrderShowViewModel>();
                cfg.CreateMap<OrderDetail, OrderDetailShowViewModel>();
                cfg.CreateMap<SystemConfig, SystemConfigViewModel>();
                cfg.CreateMap<ProductBrand, ProductBrandViewModel>();
                cfg.CreateMap<ProductUnit, ProductUnitViewModel>();
                cfg.CreateMap<Feedback, FeedbackViewModel>();
                cfg.CreateMap<UserRating, UserRatingViewModel>();


                cfg.CreateMap<OrderDetail, OrderDetailViewModel>();
            });
        }
    }
}