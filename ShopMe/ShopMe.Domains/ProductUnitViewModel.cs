using System;
using System.Collections.Generic;

namespace ShopMe.Domains
{
    public class ProductUnitViewModel
    {
       
        public int ProductUnitID { get; set; }

        public string Name { get; set; }
        public bool Status { get; set; }

        public DateTime? CreatedDate { set; get; }

        
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

       
        public string UpdatedBy { set; get; }
        public IEnumerable<ProductViewModel> Product { get; set; }
    }
}
