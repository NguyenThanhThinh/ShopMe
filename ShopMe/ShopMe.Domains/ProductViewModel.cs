using System;

namespace ShopMe.Domains
{
    [Serializable]
    public class ProductViewModel
    {
        public int ProductID { set; get; }

       
        public string Name { set; get; }

      
        public string Alias { set; get; }

        public int ProductCappacity { get; set; }
        public int CategoryID { set; get; }
    
        public int ProductBrandID { get; set; }
        public bool Status { get; set; }
     
        public string Image { set; get; }
        public int? Quantity { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Price { set; get; }
        public DateTime? CreatedDate { set; get; }
       
        public string Description { set; get; }
        public string Content { set; get; }
        public int? ViewCount { set; get; }
        public int? ViewRating { get; set; }
        public decimal? ProductRating { get; set; }
        public int ProductUnitID { get; set; }
   
     


        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

   
        public string UpdatedBy { set; get; }

        public virtual ProductBrandViewModel ProductBrand { get; set; }

        public virtual ProductCategoryViewModel ProductCategory { set; get; }
        public virtual ProductUnitViewModel ProductUnit { get; set; }
        //public IEnumerable<ProductDetailViewModel> ProductDetail { get; set; }
    }
}
