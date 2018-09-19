using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
    [Table("UserCategorys")]
  public  class UserCategory
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(128)]
        public string UserID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CategoryID { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
