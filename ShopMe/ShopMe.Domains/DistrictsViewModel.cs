namespace ShopMe.Domains
{
    public  class DistrictsViewModel
    {
        public int ID { get; set; }
    
        public int ProvinceID { get; set; }
       
        public string Name { get; set; }
        public virtual ProvincesViewModel Provinces { get; set; }
    }
}
