using System.ComponentModel.DataAnnotations;

namespace ShopMe.Domains
{
    public class SystemConfigViewModel
    {
        public int ID { set; get; }

        [Required]  
        public string Code { set; get; }

        public string ValueString { set; get; }

        public int? ValueInt { set; get; }
    }

    public class ShopInfoViewModel
    {
        public string ShopName { get; set; }
        public string ShopPhone { get; set; }
        public string ShopAddress { get; set; }
    }
}