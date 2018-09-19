using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ShopMe.Domains
{
    public class UserViewModel
    {
        public string Id { set; get; }

        [MaxLength(256, ErrorMessage = Common.CommonNotification.ERROR_MESSAGE_MAX_LENGTH_256)]
        public string FullName { set; get; }

        public DateTime BirthDay { set; get; }

        [DataType(DataType.EmailAddress, ErrorMessage = Common.CommonNotification.ERROR_MESSAGE_FORMAT_EMAIL)]
        [Remote("UserAlreadyExistsAsync", "Account", ErrorMessage = "Email đã tồn tại!")]
        public string Email { set; get; }

        [MinLength(6, ErrorMessage = Common.CommonNotification.ERROR_MESSAGE_MIN_LENGTH_6)]
        public string Password { set; get; }

        [Required(ErrorMessage = Common.CommonNotification.ERROR_MESSAGE_FIELD_NOT_NULL)]
        [Remote("UserAlreadyExistsAsync", "Account", ErrorMessage = "Tài khoản đã tồn tại")]
        public string UserName { set; get; }

        [DataType(DataType.PhoneNumber, ErrorMessage = Common.CommonNotification.ERROR_MESSAGE_FORMAT_PHONE)]
        public string PhoneNumber { set; get; }

        [MaxLength(256, ErrorMessage = Common.CommonNotification.ERROR_MESSAGE_MAX_LENGTH_256)]
        public string Address { set; get; }

        public bool IsDelete { get; set; }

        public IEnumerable<GroupViewModel> Groups { set; get; }
    }

    public enum DefaultAccountApplication
    {
        admin,
        appshopme
    }
}
