using System;
using System.Collections.Generic;

namespace ShopMe.Domains
{
    public class CategoryViewModel
    {
        public int ID
        {
            get; set;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentID { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { set; get; }
        public DateTime? UpdatedDate { get; set; }
        public int? DisplayOrder { get; set; }

        public string Alias { get; set; }
        public ICollection<ProductViewModel> Products { get; set; }
    }
}
