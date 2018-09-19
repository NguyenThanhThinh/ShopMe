using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
    [Table("THONGTIN_HOTRO_QD")]
   public  class ThongTinHoTro
    {
        [Key]
        public int SO_K { get; set; }
        
        public float? SO_BETA { get; set; }
    }
}
