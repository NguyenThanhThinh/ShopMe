using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
  public  class ProductShowViewModel
    {
        public int ProductID { set; get; }

        public string Name { set; get; }
        public string Alias { get; set; }

        public int CategoryID { set; get; }
      

 
        public string Image { set; get; }

        public decimal Price { set; get; }
        public int? Quantity { get; set; }
        public int? ViewCount { set; get; }
        public int? ViewRating { get; set; }

        public decimal? ProductRating { get; set; }


        
    }
}
