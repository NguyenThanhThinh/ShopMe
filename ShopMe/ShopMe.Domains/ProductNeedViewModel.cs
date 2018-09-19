namespace ShopMe.Domains
{
   public class ProductNeedViewModel
    {
   
        public int ProductID { get; set; }
      
        public int NeedID { get; set; }
        public virtual NeedViewModel Need { get; set; }
        public virtual ProductViewModel Product { get; set; }
    }
}
