using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
    [Table("Roles")]
    public class Role : IdentityRole
    {
        public Role() : base()
        {

        }
        [StringLength(256)]
        public string Description { set; get; }
    }
}
