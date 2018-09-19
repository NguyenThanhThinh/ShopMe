using System.Collections.Generic;

namespace ShopMe.Domains
{
  public  class SupplierViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public ICollection<ProductViewModel> Products { get; set; }
    }
}
