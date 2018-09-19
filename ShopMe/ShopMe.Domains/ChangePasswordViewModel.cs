using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Domains
{
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Chưa nhập mật khẩu cũ.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Chưa nhập mật khẩu mới.")]
        [StringLength(100, ErrorMessage = "Mật khẩu ít nhất 6 ký tự", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Chưa nhập lại mật khẩu.")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không trùng khớp.")]
        public string ConfirmPassword { get; set; }
    }
}
