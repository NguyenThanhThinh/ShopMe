namespace ShopMe.Domains
{
  public  class ProductTypeSkinViewModel
    {
        public int ProductID { get; set; }
        
        public int TypeSkinID { get; set; }
        public virtual TypeSkinViewModel TypeSkin { get; set; }
        public virtual ProductViewModel Product { get; set; }
    }
}
