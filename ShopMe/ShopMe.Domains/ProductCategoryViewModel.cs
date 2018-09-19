using System;
using System.Collections.Generic;

namespace ShopMe.Domains
{
   public class ProductCategoryViewModel
    {
        public int CategoryID { set; get; }

      
        public string Name { set; get; }

       
        public string Alias { set; get; }

     
        public string Description { set; get; }
        public int? ParentID { set; get; }
        public int? DisplayOrder { set; get; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

       
        public string UpdatedBy { set; get; }

        public IEnumerable<ProductViewModel> ProductViewModel { get; set; }
    }
}
