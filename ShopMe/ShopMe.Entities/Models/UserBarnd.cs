using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
    [Table("UserBarnd")]
   public class UserBarnd
    {
        [Key]
        [Column(Order = 0)]
        public string UserID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int ProductBrandID { get; set; }
        public virtual ProductBrand ProductBrand { get; set; }
    }
}
