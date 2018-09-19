namespace ShopMe.Domains
{
  public  class ProductDetailViewModel
    {
        public int ProductDetailID { get; set; }
    
        public int ProductID { get; set; }

        public decimal? Price { get; set; }
        public string ProductCapacity { get; set; }
        public virtual ProductViewModel Product { get; set; }
    }
}
