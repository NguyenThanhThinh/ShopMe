using System.ComponentModel.DataAnnotations;

namespace ShopMe.Domains
{
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string FullName { get; set; }
        public string HomeTown { get; set; }
        public System.DateTime? BirthDate { get; set; }
    }
}
