using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopMe.Domains
{
    public class RegisterViewModel
    {
         [DisplayName("Tên người dùng")]
        [Required(ErrorMessage = "Bạn cần nhập tên.")]
        public string FullName { set; get; }

        [DisplayName("Tài khoản")]
        [Required(ErrorMessage = "Bạn cần nhập tên đăng nhập.")]
        public string UserName { set; get; }
        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { set; get; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Bạn cần nhập email.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không đúng.")]
        public string Email { set; get; }
        [DisplayName("Địa chỉ")]
        public string Address { set; get; }
        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Bạn cần nhập số điện thoại.")]
        public string PhoneNumber { set; get; }
    }
}
