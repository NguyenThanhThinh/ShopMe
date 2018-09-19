using System;

namespace ShopMe.Domains
{
   public class StatisticPriceQuantity
    {
        public DateTime Date { get; set; }

        public decimal Revenues { get; set; }

        public decimal Benefit { get; set; }
    }
    public class ProductStatisticViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Quantities { get; set; }
        public decimal PriceAvg { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Benefit { get; set; }
    }

   
    public enum DefaultListSort
    {
        Poorly_Selling_Product = 0,
        More_Selling_Product = 1,
        Benefit_Low = 2,
        Benefit_High = 3
    }
  
}
