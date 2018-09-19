namespace ShopMe.Domains
{
    public class RatingProductViewModel
    {
     
        public string UserID { get; set; }
        public int ProductID { get; set; }
        public double? Rating { get; set; }
        public virtual ProductViewModel Product { get; set; }
        public virtual UserViewModel User { get; set; }
    }
}
