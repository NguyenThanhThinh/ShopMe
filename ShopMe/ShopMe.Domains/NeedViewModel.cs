using System.Collections.Generic;

namespace ShopMe.Domains
{
   public class NeedViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<CustomerNeedViewModel> CustomerNeed { get; set; }
        public ICollection<ProductNeedViewModel> ProductNeed { get; set; }
    }
}
