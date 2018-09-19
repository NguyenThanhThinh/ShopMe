namespace ShopMe.Domains
{
  public  class CustomerNeedViewModel
    {
     
        public int CustomerID { get; set; }
    
        public int NeedID { get; set; }

        public virtual NeedViewModel Need { get; set; }
    }
}
