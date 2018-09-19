using System;

namespace ShopMe.Domains
{
    public class HistoryViewModel
    {
        public int OrderID { get; set; }
        public int Quantity { set; get; }

        public decimal Price { set; get; }
        public DateTime? CreatedDate { set; get; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public string PaymentStatus { set; get; }
    }
}