using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMe.Entities.Models
{
    [Table("ProductBrands")]
    public class ProductBrand
    {
        [Key] public int ProductBrandID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [Required] [MaxLength(256)] public string Alias { set; get; }

        public string Description { set; get; }
        public DateTime? CreatedDate { set; get; }

        [MaxLength(256)] public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [MaxLength(256)] public string UpdatedBy { set; get; }

        public IEnumerable<Product> Products { get; set; }
    }
}