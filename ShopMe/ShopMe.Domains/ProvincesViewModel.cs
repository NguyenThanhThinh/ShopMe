using System.Collections.Generic;

namespace ShopMe.Domains
{
   public class ProvincesViewModel
    {
        public int ID { get; set; }
       
        public string Name { get; set; }
        public ICollection<DistrictsViewModel> Districts { get; set; }
    }
}
