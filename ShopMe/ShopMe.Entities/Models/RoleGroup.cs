using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
    [Table("RoleGroups")]
    public class RoleGroup
    {
        [Key]
        [Column(Order = 1)]
        public int GroupId { set; get; }

        [Key]
        [Column(Order = 2)]
        [StringLength(128)]
        public string RoleId { set; get; }

        [ForeignKey("RoleId")]
        public virtual Role Role { set; get; }

        [ForeignKey("GroupId")]
        public virtual Group Group { set; get; }
    }
}
