namespace ShopMe.Domains
{
    public  class UserRatingViewModel
    {
   
        public string UserID { get; set; }
        public int ProductID { get; set; }
        public double? Rating { get; set; }
        public virtual ProductViewModel Product { get; set; }
        public virtual UserViewModel User { get; set; }
    }
}
