using System.ComponentModel.DataAnnotations;
using ShopMe.Entities.Models;

namespace ShopMe.Domains
{
    public class OrderDetailCart
    {
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc")]
        public int ProductID { set; get; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hon 0.")]
        public int Quantity { set; get; }

        public decimal Price { get; set; }
    }

    public class OrderDetailShowViewModel : OrderDetailCart
    {
        [Required]
        public int OrderID { set; get; }

        public virtual Order Order { set; get; }

        public virtual Product Product { set; get; }
    }
}