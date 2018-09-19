using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
   public class ProductDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductDetailID { get; set; }
        [Required]
        public int ProductID { get; set; }
        public int CapacityID { get; set; }
        public decimal? Price { get; set; }
        public string ProductCapacity { get; set; }
        public virtual Product Product { get; set; }
        public virtual Capacity Capacity { get; set; }
    }
}
