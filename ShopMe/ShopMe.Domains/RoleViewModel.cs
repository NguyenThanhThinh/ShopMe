using System.ComponentModel.DataAnnotations;

namespace ShopMe.Domains
{
    public class RoleViewModel
    {
        [MaxLength(128, ErrorMessage =Common.CommonNotification.ERROR_MESSAGE_MAX_LENGTH_128)]
        public string Id { set; get; }

        public string Name { set; get; }

        [MaxLength(128, ErrorMessage = Common.CommonNotification.ERROR_MESSAGE_MAX_LENGTH_128)]
        public string Description { set; get; }
    }

    public enum DefaultRoleApplication
    {
        Access,
        Add,
        Edit,
        Delete
    }
}