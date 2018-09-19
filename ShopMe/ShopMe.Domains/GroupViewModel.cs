using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopMe.Domains
{
    public class GroupViewModel
    {
        public int ID { get; set; }   

        [Required(ErrorMessage = Common.CommonNotification.ERROR_MESSAGE_FIELD_NOT_NULL)]
        [MaxLength(256,ErrorMessage =Common.CommonNotification.ERROR_MESSAGE_MAX_LENGTH_256)]
        public string Name { get; set; }

        [MaxLength(256, ErrorMessage = Common.CommonNotification.ERROR_MESSAGE_MAX_LENGTH_256)]
        public string Description { get; set; }

        public IEnumerable<RoleViewModel> Roles { set; get; }
    }

    public enum DefaultGroupApplication
    {
        Admin,
        Customer
    }
}