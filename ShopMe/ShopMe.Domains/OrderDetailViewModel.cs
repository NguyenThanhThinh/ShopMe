﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Domains
{
  public  class OrderDetailViewModel
    {
        
        public int OrderID { set; get; }

        public int ProductID { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

     
        public virtual OrderViewModel Order { set; get; }

      
        public virtual ProductViewModel Product { set; get; }
    }
}
