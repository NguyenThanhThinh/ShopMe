using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMe.Entities.Models
{
    public class UserRating
    {
        [Key] [Column(Order = 0)] public string UserID { get; set; }
        [Key] [Column(Order = 1)] public int ProductID { get; set; }
        public double? Rating { get; set; }
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}