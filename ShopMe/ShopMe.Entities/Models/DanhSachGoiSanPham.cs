using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMe.Entities.Models
{
    [Table("SAN_PHAM_GOI_Y")]
    public class DanhSachGoiSanPham
    {
        [Key] public int ID { get; set; }
        [StringLength(128)] public string UserID { get; set; }
        public int ProductID { get; set; }
        public int Rating { get; set; }
    }
}