using System;
using System.Collections.Generic;

namespace ShopMe.Domains
{
   public class ProductBrandViewModel
    {
        public int ProductBrandID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { set; get; }
        public string Alias { set; get; }

        public string Description { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

   
        public string UpdatedBy { set; get; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
