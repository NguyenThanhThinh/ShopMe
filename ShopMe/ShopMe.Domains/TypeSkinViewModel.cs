using System.Collections.Generic;

namespace ShopMe.Domains
{
    public  class TypeSkinViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<CustomerNeedViewModel> CustomerNeed { get; set; }
        public ICollection<ProductTypeSkinViewModel> ProductTypeSkin { get; set; }
    }
}
