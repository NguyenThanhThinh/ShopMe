using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopMe.Domains
{
  public  class ShoppingCartAdmin
    {
        [Required(ErrorMessage = "Chưa nhập họ tên.")]
        [MaxLength(256, ErrorMessage = "Họ tên không quá 256 ký tự.")]
        public string CustomerName { set; get; }

        [MaxLength(256, ErrorMessage = "Địa chỉ thanh toán không quá 256 ký tự.")]
        public string CustomerBillingAddress { set; get; }

        [Required(ErrorMessage = "Chưa nhập Email.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không đúng định dạng.")]
        [MaxLength(256, ErrorMessage = "Email không quá 256 ký tự.")]
        public string CustomerEmail { set; get; }

        [Required(ErrorMessage = "Chưa nhập số điện thoại.")]
        [MaxLength(50, ErrorMessage = "Số điện thoại không quá 50 ký tự.")]
        public string CustomerMobile { set; get; }

        [MaxLength(500, ErrorMessage = "Lời nhắn không quá 500 ký tự.")]
        public string CustomerMessage { set; get; }

        [MaxLength(256, ErrorMessage = "Phương thức thanh toán không quá 256 ký tự.")]
        public string PaymentMethod { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        [Required(ErrorMessage = "Địa chỉ giao hàng là bắt buộc")]
        [MaxLength(256, ErrorMessage = "Địa chỉ giao hàng không quá 256 ký tự.")]
        public string CustomerAddress { get; set; }
        [MaxLength(256, ErrorMessage = "Mã tài khoản không quá 256 ký tự.")]
        public string CustomerId { get; set; }

       
        public virtual IEnumerable<OrderDetailCart> OrderDetails { get; set; }
    }
}
