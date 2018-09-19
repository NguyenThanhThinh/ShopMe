using System;
using System.Collections.Generic;

namespace ShopMe.Domains
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }

        public string CustomerName { set; get; }


        public string CustomerAddress { set; get; }


        public string CustomerEmail { set; get; }

        public string CustomerMobile { set; get; }


        public string CustomerProvince { set; get; }

        public string CustomerDistricts { set; get; }


        public string PaymentMethod { set; get; }

        public string UserID { get; set; }
        public DateTime? CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public int? PaymentStatus { set; get; }
        public int? Status { set; get; }
        public string BankCode { set; get; }
        public virtual UserViewModel User { get; set; }
        public virtual IEnumerable<OrderDetailViewModel> OrderDetail { set; get; }
    }
}