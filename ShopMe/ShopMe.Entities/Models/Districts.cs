using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMe.Entities.Models
{
    [Table("Districts")]
    public class Districts
    {
        [Key] public int ID { get; set; }
        [ForeignKey("Provinces")] public int ProvinceID { get; set; }
        [MaxLength(256)] public string Name { get; set; }
        public virtual Provinces Provinces { get; set; }
    }
}