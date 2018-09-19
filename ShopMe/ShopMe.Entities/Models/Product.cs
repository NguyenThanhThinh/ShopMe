using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
  public  class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Required]
        [MaxLength(256)]
        public string Alias { set; get; }

        [Required]
        public int CategoryID { set; get; }
        [Required]
        public int ProductBrandID { get; set; }
        [Required]
 
        public int ProductUnitID { get; set; }
        public bool Status { get; set; }
        [MaxLength(256)]
        public string Image { set; get; }
        public decimal OriginalPrice { get; set; }
        public decimal Price { set; get; }
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { set; get; }
        [MaxLength(500)]
        public string Description { set; get; }
        public string Content { set; get; }
        public int? ViewCount { set; get; }
        public int? ViewRating { get; set; }
        public int ProductCappacity { get; set; }
     
        public decimal? ProductRating { get; set; }
     
       
        [MaxLength(256)]
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [MaxLength(256)]
        public string UpdatedBy { set; get; }


        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ProductBrand ProductBrand { get; set; }
        public virtual ProductUnit ProductUnit { get; set; }
        //public IEnumerable<ProductDetail> ProductDetail { get; set; }



    }
}
