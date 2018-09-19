using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
  public  class UserRatingAverage
    {
        [Key, ForeignKey("User")]
        public string UserID { get; set; }
        public double? RatingAverage { get; set; }
        public virtual User User { get; set; }
    }
}
