using System.Collections.Generic;

namespace ShopMe.Domains
{
    public class HomeViewModel
    {
        public string ReturnUrl { get; set; }
        public IEnumerable<ProductViewModel> LastestProducts { set; get; }
        public IEnumerable<ProductViewModel> TopSaleProducts { set; get; }
        public IEnumerable<ProductViewModel> ProductCategoryDM { set; get; }

        public IEnumerable<ProductViewModel> ProductBrandDM { set; get; }
        public IEnumerable<ProductViewModel> ProductLike { set; get; }
        public IEnumerable<FeedbackViewModel> GetCommentRating { get; set; }
    }
}