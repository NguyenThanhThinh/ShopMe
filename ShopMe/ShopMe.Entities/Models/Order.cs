using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
  
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerName { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerEmail { set; get; }
   
        [MaxLength(50)]
        public string CustomerMobile { set; get; }

      
        
        public string CustomerProvince { set; get; }
    
     
        public string CustomerDistricts { set; get; }

        [MaxLength(256)]
        public string PaymentMethod { set; get; }
        [StringLength(128)]
        public string UserID { get; set; }
        public DateTime? CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public int? PaymentStatus { set; get; }
        public int? Status { set; get; }

        public virtual User User { get; set; }

        public virtual IEnumerable<OrderDetail> OrderDetail { set; get; }

    }
}
