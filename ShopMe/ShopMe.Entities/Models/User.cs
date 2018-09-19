using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ShopMe.Entities.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
        [StringLength(256)] [Required] public string FullName { set; get; }

        [StringLength(256)] public string Address { set; get; }
        [StringLength(100)] public string Provinces { get; set; }
        [StringLength(100)] public string Districts { get; set; }


        public DateTime? BirthDay { set; get; }

        public bool IsDelete { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager,
            string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual IEnumerable<Order> Orders { set; get; }
        public IEnumerable<UserCategory> UserCategory { get; set; }
        public IEnumerable<UserBarnd> UserBarnd { get; set; }
        public virtual ICollection<PearsonScore> PearsonScore { get; set; }
        public virtual ICollection<PearsonScore> PearsonScore1 { get; set; }
        public virtual UserRatingAverage UserRatingAverage { get; set; }
    }
}